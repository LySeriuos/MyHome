using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using BlazorBootstrap;
using MyHome.Models;
using MyHomeBlazorApp.BlazorData;
using MyHomeBlazorApp.Components;
using MyHomeBlazorApp.Components.Modals;
using MyHomeBlazorApp.Shared;

namespace MyHomeBlazorApp.Pages;

public partial class Details
{
    [Inject]
    private DataService DataService { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    // Devices Details page should be sorted by devices ID number by default.
    // Check IF the link to the file moves together with the device Profile!!!!!!!
    [Parameter]
    public int Tab { get; set; }
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    private int SelectedRealEstateID
    {
        get => selectedRealEstateID;
        set { selectedRealEstateID = value; }
    }

    private int TabID = 0;
    private int selectedRealEstateID = -1;
    private string _chooseOptionValue = "Choose Real Estate";
    private string? _deletedObjectName;
    private string message = "";
    private string modalTitle = "";
    public bool listHasBeenChanged = false;
    private RealEstate currentChosedRealEsate = new();
    private RealEstate currentRealEstate = new();
    private int chosenRealEsatateId;
    private Address currentRealEstateAddress = new();
    private Address shopAddress = new();
    private DeviceProfile? currentDevice { get; set; }
    private UserProfile? currentUser { get; set; }
    private IQueryable<DeviceProfile>? _devicesListAsQueryable;
    private IQueryable<RealEstate>? _realEstatesListAsQueryable;
    private List<RealEstate>? _realEstatesList { get; set; }
    // Initialize with an empty list so the UI doesn't crash while loading
    private IQueryable<DeviceProfile> _unassignedDevicesListAsQueryable = Enumerable.Empty<DeviceProfile>().AsQueryable();
    public Action? UnassignedDevicesListChanged;
    List<ToastMessage> messages = new List<ToastMessage>();
    List<DeviceProfile> chosedDevicesList { get; set; } = new();
    private void HandleQrCode(DeviceProfile targetDevice)
    {
        DataService.AddToPrintQueue(targetDevice);
        StateHasChanged();
    }

    private async Task HandleAddAllToPrint()
    {
        await DataService.AddAllToPrintQueue();
        StateHasChanged();
        await Task.CompletedTask;
    }

    private async Task NavToPrintPage()
    {
        if (DataService.GetCurrentQueue().Any())
        {
            var list = DataService.GetCurrentQueue();
            // saving the list to the local storage, otherwise list becames empty
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "printQueue", JsonSerializer.Serialize(list));
            await JSRuntime.InvokeVoidAsync("open", "qrPrinting", "_blank");
            DataService.CleanCurrentQueue();
            StateHasChanged();
        }
    }


    private async Task SendDeviceIDToCreateEmailComponent(int deviceId)
    {
        await JSRuntime.InvokeVoidAsync("open", $"createEmail/{deviceId}", "_blank");
    }

    public string deviceName
    {
        get => currentDevice.DeviceName;
        set { currentDevice.DeviceName = value; }
    }
    private CustomReusableModal _reusableModal = default!;
    private SuccessModal _refSuccessModal = default!;



    private async Task ShowDeviceEditFormInModal(int deviceID)
    {
        modalTitle = "Edit Devices Details";
        chosenRealEsatateId = DataService.GetRealEstateByDeviceID(deviceID);
        currentUser = DataService.CurrentUserWithAllData;
        currentDevice = DataService.GetDeviceById(deviceID);
        await _reusableModal.ShowAsync();
    }
    // user = await UserAccessor.GetRequiredUserAsync(HttpContext);
    private async Task OnShowModalAddNewDevice()
    {
        modalTitle = "Add New Device";
        currentUser = DataService.CurrentUserWithAllData;
        currentDevice = new DeviceProfile
        {
            DeviceID = 0,
            DeviceName = "",
            DeviceType = DeviceType.Other
        };
        currentChosedRealEsate.RealEstateID = 0;
        chosenRealEsatateId = 0;
        StateHasChanged();        
        await _reusableModal.ShowAsync();       
    }

    private async Task NavigateToExtraWarrantyOrReceiptFile(int deviceID, string relativePath)
    {
        string fileUrl = DataService.GetFileUrl(relativePath, deviceID);
        if (!string.IsNullOrEmpty(fileUrl))
        {
            await JSRuntime.InvokeVoidAsync("open", fileUrl, "_blank");
        }
    }


    private async Task OnModalDeleteSuccess()
    {
        await refModalDeleteSuccess.OnShowModalClick();
    }

    #region Real Estate

    private ModalEditAddRealEstate refModalEditAddRealEstate = default!;
    private ModalDeleteRealEstate refModalRealEstateDelete = default!;

    private void RealEstateDataHasChanged()
    {
        _realEstatesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.AsQueryable();
        StateHasChanged();
    }

    private async Task OnShowModalRealEstateClick(int realEstateID)
    {
        RealEstate? targetRealEstate = DataService.GetRealEstate(realEstateID);
        if (targetRealEstate is null)
        {
            Console.WriteLine(
                $"Error: RealEstate ID {realEstateID} not found in DataService.");

            return;
        }
        currentRealEstate = targetRealEstate;
        currentRealEstateAddress = targetRealEstate.Address;

        modalTitle = "Edit Real Estate";
        await refModalEditAddRealEstate.OnShowModalEditAddRealEstateClick();
        
    }

    private async Task OnShowModalAddNewRealEstate()
    {
        modalTitle = "Add New Real Estate";
        currentRealEstate = new();
        currentRealEstateAddress = new();
        currentRealEstateAddress = currentRealEstate.Address;
        await refModalEditAddRealEstate.OnShowModalEditAddRealEstateClick();
    }

    private async Task OnShowModalChooseOptionDeleteRealEstate(int currentRealEstateID)
    {
        currentUser = DataService.CurrentUserWithAllData;
        _realEstatesList = currentUser.RealEstates;

        currentRealEstate = DataService.GetRealEstate(currentRealEstateID);

        if (currentRealEstate == null || currentRealEstate.RealEstateID == 0)
        {
            Console.WriteLine($"RealEstate ID {currentRealEstateID} not found.");
            return;
        }

        _deletedObjectName = currentRealEstate.RealEstateName;
        await refModalRealEstateDelete.OnShowModalDeleteRealEstateClick();
    }


    #endregion

    #region Devices

    private ModalDeleteSuccessful refModalDeleteSuccess = default!;
    private ModalConfirmDeviceDeletion refModalConfirmDeviceDeletion = default!;

    // private async Task OnModalReassignDevicesRequest(int realEstateId)
    // {
    //     // this Id will be passed to other modal to be able to move devices list
    //     await refModalReasignDevices.OnShowModalReasignDevices(realEstateId);
    // }

    private async Task DeviceDataHasChanged()
    {
        // _devicesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).AsQueryable();
        var unassignedList = await DataService.GetUserWithUnassignedDevicesListAsync();
        _unassignedDevicesListAsQueryable = unassignedList.AsQueryable();
        var allDevicesList = await DataService.GetAllUserDevicesAsync();
        _devicesListAsQueryable = allDevicesList.AsQueryable();
        StateHasChanged();
    }

    private async Task HandleUnnassignedDevicesListDataChanged()
    {
        // Re-fetch the unassigned list so .Any() becomes false
        var unassigned = await DataService.GetUserWithUnassignedDevicesListAsync();
        _unassignedDevicesListAsQueryable = unassigned.AsQueryable();
        var allDevicesList = await DataService.GetAllUserDevicesAsync();
        _devicesListAsQueryable = allDevicesList.AsQueryable();
        _realEstatesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.AsQueryable();
        // currentUser = await DataService.GetFullUserAsync();

        StateHasChanged();
    }

    private async Task OnShowModalDeleteDeviceClick(int deviceId)
    {
        var deviceTarget = DataService.CurrentUserWithAllData.GetAllDevices()
        .FirstOrDefault(d => d.DeviceID == deviceId);

        if (deviceTarget != null)
        {
            currentDevice = deviceTarget;
            _deletedObjectName = deviceTarget.DeviceName;

            StateHasChanged();

            if (refModalConfirmDeviceDeletion != null)
            {
                await refModalConfirmDeviceDeletion.OnShowModalClick();
            }
        }
    }

    #endregion

    #region Warranties

    private ModalEditWarrantyDetails _refModalEditWarrantyDetails = default!;

    private void WarrantyDataHasChanged()
    {
        _devicesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).AsQueryable();
        StateHasChanged();
    }

    private async Task OnShowModalEditAddDeviceWarrantyClick(int deviceID)
    {
        currentDevice = DataService.GetDeviceById(deviceID);
        await _refModalEditWarrantyDetails.OnShowModalEditWarrantyClick();
    }

    #endregion

    #region Shops

    private ModalEditShopDetails refModalEditShopDetails = default!;

    private void ShopDataHasChanged()
    {
        _realEstatesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.AsQueryable();
        StateHasChanged();
    }

    private async Task OnShowModalEditAddShopClick(int deviceID)
    {
        currentDevice = DataService.GetDeviceById(deviceID);
        shopAddress = currentDevice.DeviceWarranty.Shop.Address;
        await refModalEditShopDetails.OnShowModalEditAddShopClick();
    }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        await DataService.InitializedUserAsync();
        var list = await DataService.GetUserWithUnassignedDevicesListAsync();
        _unassignedDevicesListAsQueryable = list.AsQueryable();
        _devicesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.
        SelectMany(realEstate => realEstate.DevicesProfiles)
        .OrderBy(d => DataService.GetRealEstateByDeviceID(d.DeviceID))
        .ThenBy(d => d.DeviceID).AsQueryable();

        _realEstatesListAsQueryable = DataService.CurrentUserWithAllData.RealEstates.AsQueryable();
        // File availability must not mutate saved document references.
        // 1. Call the method to get the combined and "stamped" list
        List<DeviceProfile>
    allDevices = await DataService.GetAllUserDevicesAsync();

        // 2. Convert it to IQueryable so QuickGrid sorting works perfectly!
        _devicesListAsQueryable = allDevices.AsQueryable();
        // chosedDevicesList = DataService.GetQueueAndClear();


        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (Tab == 1)
        {
            TabID = 1;
        }
        else if (Tab == 2)
        {
            TabID = 2;
        }
        else if (Tab == 3)
        {
            TabID = 3;
        }
        else
        {
            message = "There is no option";
        }
    }





    private void HandleRemoveQueuedDevice(DeviceProfile device)
    {
        DataService.RemoveFromQueue(device);
    }
}
