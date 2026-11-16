using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using My_Home;
using MyHome;
using MyHome.Models;
using System.Xml;
//using System.Data.Entity;

namespace MyHomeBlazorApp.BlazorData
{
    public partial class DataService
    {

        /// <summary>
        /// Method to add new device in to database
        /// </summary>
        /// <param name="deviceToAdd">Device object to be add</param>
        /// <param name="chosedRealEstateID">Real Estate Id to add new device into</param>
        /// <returns></returns>
        public async Task AddNewDeviceToDataBaseAsync(DeviceProfile deviceToAdd, int chosedRealEstateID)
        {
            await EnsureUserDataLoadedAsync();
            UserProfile currentUser = _currentUserWithAllData
    ?? throw new InvalidOperationException("User data could not be loaded.");
            deviceToAdd.DeviceWarranty ??= new();
            deviceToAdd.DeviceWarranty.Shop ??= new();
            deviceToAdd.DeviceWarranty.Shop.Address ??= new();
            deviceToAdd.TempRealEstateName = chosedRealEstateID.ToString();
            // if user has not created any real estates so the device will be added to unnassigned list
            if (chosedRealEstateID == 0)
            {
                _currentUserWithAllData.UnassignedDevicesList.Add(deviceToAdd);
            }
            // if user has real estate, device will be added to chosed real estate
            else
            {
                RealEstate? chosedRealEstate = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == chosedRealEstateID);

                if (chosedRealEstate == null)
                {
                    throw new Exception("Selected Real Estate not found.");
                }

                chosedRealEstate.DevicesProfiles.Add(deviceToAdd);
            }
        }

        public async Task<List<DeviceProfile>> GetAllUserDevicesAsync()
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }
            // Get the raw lists
            var assigned = _currentUserWithAllData.RealEstates
                .SelectMany(re => re.DevicesProfiles).ToList();
            var unassigned = _currentUserWithAllData.UnassignedDevicesList;

            var allDevices = assigned.Concat(unassigned).ToList();

            // Loop through and add realEstate ID or unnassigned based on if devices is assigned or not to the Real Estate
            foreach (var device in allDevices)
            {
                int realEstateId = GetRealEstateByDeviceID(device.DeviceID);

                if (realEstateId != 0)
                {   // combining real estate name and the id 
                    var realEstate = GetRealEstate(realEstateId);
                    device.TempRealEstateName = realEstate?.RealEstateName + " / " + realEstateId;
                }
                else
                {
                    // if real estate is not assigned it gives unnassigned and id 0
                    device.TempRealEstateName = "Unassigned / 0";
                }
            }
            return allDevices;
        }


        /// <summary>
        /// Method to remove device from DB
        /// </summary>
        /// <param name="deviceToDelete">Object to delete</param>
        /// <returns></returns>
        public async Task RemoveDeviceFromDb(DeviceProfile deviceToDelete)
        {
            // Query the specific Device directly from its own table with its exact child dependencies
            var fullDevice = await _dbcontext.Set<DeviceProfile>()
                .Include(d => d.DeviceWarranty)
                    .ThenInclude(w => w.Shop)
                        .ThenInclude(s => s.Address)
                .FirstOrDefaultAsync(d => d.DeviceID == deviceToDelete.DeviceID);

            if (fullDevice != null)
            {
                // Manually remove the child structures from the bottom up to clear the database safely
                if (fullDevice.DeviceWarranty != null)
                {
                    if (fullDevice.DeviceWarranty.Shop != null)
                    {
                        if (fullDevice.DeviceWarranty.Shop.Address != null)
                        {
                            _dbcontext.Remove(fullDevice.DeviceWarranty.Shop.Address);
                        }
                        _dbcontext.Remove(fullDevice.DeviceWarranty.Shop);
                    }
                    _dbcontext.Remove(fullDevice.DeviceWarranty);
                }

                // Remove the main device record
                _dbcontext.Remove(fullDevice);
                await UpdateObjectInDB();

            }
        }

        /// <summary>
        /// Getting last item in the sequence
        /// </summary>
        /// <returns>Returns last added device in the list</returns>
        public DeviceProfile? LastAddedDevice()
        {
            return Devices?.MaxBy(d => d.DeviceID);
        }

        /// <summary>
        /// Method to move devices list from one RealEstate to another
        /// </summary>
        /// <param name="realEstateID">Devices list will be moved into RealEstate by realEstateID</param>
        /// <param name="currentRealEstate">RealEstate to move from(delete) devices list</param>
        public async Task MoveDevicesListToOtherRealEstate(int targetRealEstateId, RealEstate currentRealEstate)
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            RealEstate? targetRealEstate = _currentUserWithAllData.RealEstates
        .FirstOrDefault(r => r.RealEstateID == targetRealEstateId);
            if (targetRealEstate == null || currentRealEstate == null)
            {
                return;
            }

            List<DeviceProfile>? devicesToMove = currentRealEstate.DevicesProfiles.ToList();


            foreach (DeviceProfile deviceProfile in currentRealEstate.DevicesProfiles.ToList())
            {
                targetRealEstate.DevicesProfiles.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            await UpdateObjectInDB();
        }

        /// <summary>
        /// Method to move DeviceProfile from one Real Estate to another
        /// </summary>
        /// <param name="deviceToMoveID">DeviceProfile ID which will be moved </param>
        /// <param name="_currentUserWithAllData">Indentified user</param>
        /// <param name="realEstateIdToAddDevice">Real Estate ID to move in Device by deviceToMoveID</param>
        public async Task MoveDeviceToOtherRealEstateAsync(int deviceToMoveID, int realEstateIdToAddDevice)
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            int realEstateIdToMoveFrom = GetRealEstateByDeviceID(deviceToMoveID);
            DeviceProfile? deviceToMove = _currentUserWithAllData.RealEstates
        .SelectMany(r => r.DevicesProfiles)
        .FirstOrDefault(d => d.DeviceID == deviceToMoveID);

            RealEstate? realEstateToMoveFrom = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToMoveFrom);
            RealEstate? realEstateToAddDevice = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToAddDevice);
            if (realEstateToMoveFrom == null) return;
            if (realEstateToAddDevice != null)
            {
                realEstateToMoveFrom.DevicesProfiles.Remove(deviceToMove);
                realEstateToAddDevice.DevicesProfiles.Add(deviceToMove);
            }

        }

        public void MoveDeviceToUnnassignedList(DeviceProfile targetDevice)
        {
            if (targetDevice != null || targetDevice.DeviceID != 0)
            {
                int realEstateIdToMoveFrom = GetRealEstateByDeviceID(targetDevice.DeviceID);
                RealEstate? realEstateToMoveFrom = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToMoveFrom);
                CurrentUserWithAllData.UnassignedDevicesList.Add(targetDevice);
                realEstateToMoveFrom.DevicesProfiles.Remove(targetDevice);

            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_currentUserWithAllData"></param>
        /// <param name="realEstateToAddDevice"></param>
        /// <param name="currentDevice"></param>
        /// <returns></returns>
        public void MoveDeviceFromUnassignedDevicesProfile(UserProfile _currentUserWithAllData, int realEstateToAddDevice, DeviceProfile currentDevice)
        {
            _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateToAddDevice).DevicesProfiles.Add(currentDevice);
            _currentUserWithAllData.UnassignedDevicesList.Remove(currentDevice);
        }

        /// <summary>
        /// Method to add device ID in the list of selected devices to print QR codes. This list will be used in the page to print QR codes for selected devices. If device ID is already in the list so it will not be added again.
        /// </summary>
        /// <param name="deviceId">Selected Device ID</param>
        public DeviceProfile GetDeviceById(int id)
        {
            DeviceProfile currentDevice = new();
            int i = 0;
            DeviceProfile? currentDeviceTest = new DeviceProfile();
            DeviceProfile device;
            for (i = 0; i < _currentUserWithAllData.GetAllDevices().Count; i++)
            {
                device = _currentUserWithAllData.GetAllDevices()[i];
                if (device.DeviceID == id)
                {
                    currentDevice = device;
                    break;
                }
            }
            return currentDevice;
        }

        public async Task<DeviceProfile?> GetDeviceForGuestAsync(int userId, int deviceId)
        {
            // 1. Fetch User with BOTH potential device paths included
            var user = await _dbcontext.Users
                .Include(u => u.UserProfile)
                    .ThenInclude(p => p.UnassignedDevicesList) // Path A
                        .ThenInclude(d => d.DeviceWarranty)
                .Include(u => u.UserProfile)
                    .ThenInclude(p => p.RealEstates)           // Path B
                        .ThenInclude(r => r.DevicesProfiles)
                            .ThenInclude(d => d.DeviceWarranty)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserProfile != null && u.UserProfile.UserID == userId);

            if (user?.UserProfile == null) return null;

            // 2. Check the Unassigned List first
            var unassignedDevice = user.UserProfile.UnassignedDevicesList?
                .FirstOrDefault(d => d.DeviceID == deviceId);

            if (unassignedDevice != null) return unassignedDevice;

            // 3. If not there, check the Real Estate collections
            var assignedDevice = user.UserProfile.RealEstates?
                .SelectMany(r => r.DevicesProfiles)
                .FirstOrDefault(d => d.DeviceID == deviceId);

            return assignedDevice;
        }

        public async Task Navigate(DeviceProfile currentDevice, IJSRuntime jSRuntime)
        {
            var query = new Dictionary<string, string>
            {
            { $"{currentDevice.DeviceProduser}", $"{currentDevice.DeviceModelNumber}" }
        };
            string buildedUrl = Util.BuildUrlWithQueryStringUsingStringConcat(Program.Constants.BASE_API_URL, query);
            await jSRuntime.InvokeVoidAsync("open", buildedUrl, "_blank");
        }

        public string? GetManualUrl(DeviceProfile currentDevice)
        {
            if (string.IsNullOrWhiteSpace(currentDevice.DeviceProduser) ||
                string.IsNullOrWhiteSpace(currentDevice.DeviceModelNumber))
            {
                return null;
            }

            var searchText =
                $"{currentDevice.DeviceProduser.Trim()} " +
                $"{currentDevice.DeviceModelNumber.Trim()} manual";

            return $"{Program.Constants.BASE_API_URL}" +
                   $"?q={Uri.EscapeDataString(searchText)}";
        }
    }
}
