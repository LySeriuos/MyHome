using BlazorBootstrap;
using com.sun.org.apache.xml.@internal.resolver.helpers;
using java.util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using My_Home;
using My_Home.Models;
using MyHome;
using Microsoft.Data.Sqlite;
using MyHome.Models;
using MyHomeBlazorApp.Pages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Data.Entity;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using Unity;
using static com.sun.management.VMOption;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MailKit.Search;
using Unity.Injection;


namespace MyHomeBlazorApp.BlazorData

{

    public class DataService
    {
        public DataService(MyHomeBlazorAppContext dbcontext, UserManager<MyHomeBlazorAppUser> usermanager, AuthenticationStateProvider authenticationStateProvider)
        {
            _userManager = usermanager;
            //_users = Data.GetUsersListFromXml(_path);
            _authenticationStateProvider = authenticationStateProvider;
            _dbcontext = dbcontext;
            CurrentAppUser = GetCurrentUser().Result;
            _currentUserWithData = GetDbUserDeviceProfileWithWarrantyShopAddressData().Result.UserProfile;
            //UnassignedDevicesList = GetUserWithUnassignedDevicesList().Result.ToList();
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(_currentUserWithData, 180);
            FirstExpiringDevice = FirstExpiringWarranty();
            DevicesWarranties = Logic.GetUserDevicesWarranties(_currentUserWithData);
        }

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private UserManager<MyHomeBlazorAppUser> _userManager;
        private MyHomeBlazorAppContext _dbcontext;
        private UserProfile _currentUserWithData = new();
        public UserProfile CurrentUserWithAllData => _currentUserWithData;
        public MyHomeBlazorAppUser CurrentAppUser;
        public List<DeviceProfile>? Devices => _currentUserWithData.GetAllDevices();
        public List<DeviceProfile>? ExpiringDevices { get; set; } = new List<DeviceProfile>();
        public DeviceProfile? Device { get; set; } = new DeviceProfile();
        public RealEstate? CurrentRealEstate { get; set; } = new RealEstate();
        public Address? Adrress { get; set; } = new Address();
        public DeviceWarranty? DeviceWarranty { get; set; } = new DeviceWarranty();
        public DeviceProfile? FirstExpiringDevice { get; set; } = new DeviceProfile();
        public List<DeviceWarranty>? DevicesWarranties { get; set; } = new List<DeviceWarranty>();
        public DeviceProfile? CurrentDevice { get; set; } = new DeviceProfile();
        public Shop? Shop { get; set; } = new Shop();
        public List<DeviceProfile>? UnassignedDevicesList { get; set; }

        #region User

        public async Task<MyHomeBlazorAppUser> GetCurrentUser()
        {
            var AuthSate = _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = AuthSate.Result.User;
            if (user.Identity.IsAuthenticated)
            {
                CurrentAppUser = await _userManager.GetUserAsync(user);
            }
            return CurrentAppUser;
        }

        public async Task<List<DeviceProfile>> GetUserWithUnassignedDevicesList()
        {
            var userWithUnnasignedListData = _dbcontext.Users.Include(u => u.UserProfile)
                .ThenInclude(u => u.UnassignedDevicesList)
                .FirstOrDefault(u => u.Id == CurrentAppUser.Id);
            if (userWithUnnasignedListData != null)
            {
                UnassignedDevicesList = userWithUnnasignedListData.UserProfile.UnassignedDevicesList;
            }
            else
            {
                UnassignedDevicesList = new List<DeviceProfile>();
            }
            return UnassignedDevicesList;
        }

        public async Task<UserProfile> CurrentDbUserWithRealEstatesData()
        {
            var userWithData = _dbcontext.Users.Include(u => u.UserProfile)
                .ThenInclude(p => p.RealEstates)
                .FirstOrDefault(u => u.Id == CurrentAppUser.Id);
            UserProfile userToReturn = userWithData.UserProfile;
            return userToReturn;
        }

        public Task<MyHomeBlazorAppUser> GetDbUserWithRealEstateAddressData()
        {
            var userWithAddressData = _dbcontext.Users.Include(u => u.UserProfile)
                .ThenInclude(r => r.RealEstates)
                .ThenInclude(a => a.Address)
                .FirstOrDefault(u => u.Id == CurrentAppUser.Id);
            return Task.FromResult<MyHomeBlazorAppUser>(userWithAddressData);
        }

        public async Task<MyHomeBlazorAppUser> GetDbUserDeviceProfileWithWarrantyShopAddressData()
        {
            var userWithDevicesData = _dbcontext.Users.Include(u => u.UserProfile)
                .ThenInclude(r => r.RealEstates).ThenInclude(r => r.DevicesProfiles)
                .ThenInclude(d => d.DeviceWarranty).ThenInclude(dw => dw.Shop)
                .ThenInclude(shop => shop.Address)
                .FirstOrDefault(u => u.Id == CurrentAppUser.Id);
            if (userWithDevicesData != null)
            {
                return userWithDevicesData;
            }
            else
            {
                return new MyHomeBlazorAppUser();
            }
        }

        #endregion

        #region Real Estate

        /// <summary>
        /// Get RealEstate object by realEstate ID
        /// </summary>
        /// <param name="realEstateID">RealEstate ID to look for</param>
        /// <returns>RealEstate object</returns>
        public RealEstate GetRealEstate(int realEstateID)
        {
            RealEstate realEstateById = new RealEstate();
            if (_currentUserWithData.RealEstates.Count > 0 && _currentUserWithData.RealEstates.Any(item => item.RealEstateID == realEstateID))
            {
                realEstateById = _currentUserWithData.RealEstates.First(RealEstates => RealEstates.RealEstateID == realEstateID);
            }
            else
            {
                realEstateById = new();
            }
            return realEstateById;
        }

        /// <summary>
        /// Method to save created new RealEstate
        /// </summary>
        /// <param name="realEstate">Created RealEstate</param>
        
        
        public async Task AddNewRealEstateToDB(RealEstate currentRealEstate)
        {
            //check if incoming realestate object has id 0, otherwise error 
            if (currentRealEstate.RealEstateID == 0)
            {
                //currentRealEstate.Address;
                _currentUserWithData.RealEstates.Add(currentRealEstate);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Wrong RealEstate ID", nameof(currentRealEstate.RealEstateID));
            }
        }

        /// <summary>
        /// Method to get RealEstate object by Device ID parameter
        /// </summary>
        /// <param name="deviceId">Parameter</param>
        /// <returns>RealEstate's ID</returns>
        public int GetRealEstateByDeviceID(int deviceId)
        {
            RealEstate currentRealEstateTest = new();
            foreach (RealEstate realEstate in _currentUserWithData.RealEstates)
            {
                foreach (DeviceProfile device in realEstate.DevicesProfiles)
                {
                    if (device.DeviceID == deviceId)
                    {
                        currentRealEstateTest = realEstate;
                        break;
                    }
                }
            }
            return currentRealEstateTest.RealEstateID;
        }

        /// <summary>
        /// To get last added RealEstate in RealEstates list
        /// </summary>
        /// <returns>Last added RealEstate in the list</returns>
        public RealEstate LastAddedRealEstate()
        {
            RealEstate lastRealEstate;
            if (_currentUserWithData.RealEstates.Count == 0)
            {
                lastRealEstate = new RealEstate();
            }
            else
            {
                lastRealEstate = _currentUserWithData.RealEstates.Last();
            }
            return lastRealEstate;
        }

        /// <summary>
        /// Method to delete(remove) RealEstate from the RealEstates list
        /// </summary>
        /// <param name="contextChosedRealEstateID">Chosed RealEstate</param>
        public async Task RemoveRealEstate(int contextChosedRealEstateID)
        {
            bool realEstateExsits = _currentUserWithData.RealEstates.Contains(GetRealEstate(contextChosedRealEstateID));
            if (realEstateExsits == true)
            {
                RealEstate realEstateToDelete = _currentUserWithData.RealEstates.First(r => r.RealEstateID == contextChosedRealEstateID);
                _dbcontext.Remove(realEstateToDelete);
                await UpdateObjectInDB();
            }
            else
            {
                return;
            }
        }

        #endregion
        #region Devices

        /// <summary>
        /// Method to add new device in to database
        /// </summary>
        /// <param name="deviceToAdd">Device object to be add</param>
        /// <param name="chosedRealEstateID">Real Estate Id to add new device into</param>
        /// <returns></returns>
        public async Task AddNewDeviceToDataBase(DeviceProfile deviceToAdd, int chosedRealEstateID)
        {
            deviceToAdd.DeviceWarranty = new();
            deviceToAdd.DeviceWarranty.Shop = new();
            deviceToAdd.DeviceWarranty.Shop.Address = new();
            RealEstate chosedRealEstate = new();
            if (chosedRealEstateID != 0)
            {
                chosedRealEstate = _currentUserWithData.RealEstates.First(r => r.RealEstateID == chosedRealEstateID);
            }

            if (_currentUserWithData.GetAllDevices().Any(d => d.DeviceID == deviceToAdd.DeviceID))
            {   //Should twrow an error that device with this ID already exists
                return;
            }
            else
            {
                chosedRealEstate.DevicesProfiles.Add(deviceToAdd);
                await _dbcontext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Method to remove device from DB
        /// </summary>
        /// <param name="deviceToDelete">Object to delete</param>
        /// <returns></returns>
        public async Task RemoveDeviceFromDb(DeviceProfile deviceToDelete)
        {
            _dbcontext.Remove(deviceToDelete);
            await UpdateObjectInDB();

        }

        /// <summary>
        /// Getting last item in the sequence
        /// </summary>
        /// <returns>Returns last added device in the list</returns>
        public DeviceProfile LastAddedDevice()
        {
            List<DeviceProfile>? devices = Devices;
            DeviceProfile? device = devices.LastOrDefault();
            return device;
        }

        /// <summary>
        /// Method to move devices list from one RealEstate to another
        /// </summary>
        /// <param name="realEstateID">Devices list will be moved into RealEstate by realEstateID</param>
        /// <param name="currentRealEstate">RealEstate to move from(delete) devices list</param>
        public async Task MoveDeviceListToOtherRealEstate(int realEstateID, RealEstate currentRealEstate)
        {
            List<DeviceProfile> deviceProfilesMoveToRealEstate = _currentUserWithData.RealEstates.First(r => r.RealEstateID == realEstateID).DevicesProfiles;
            foreach (DeviceProfile deviceProfile in currentRealEstate.DevicesProfiles.ToList())
            {
                deviceProfilesMoveToRealEstate.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            await _dbcontext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to move DeviceProfile from one Real Estate to another
        /// </summary>
        /// <param name="deviceToMoveID">DeviceProfile ID which will be moved </param>
        /// <param name="_currentUserWithData">Indentified user</param>
        /// <param name="realEstateIdToAddDevice">Real Estate ID to move in Device by deviceToMoveID</param>
        public void MoveDeviceToOtherRealEstate(int deviceToMoveID, UserProfile _currentUserWithData, int realEstateIdToAddDevice)
        {
            int realEstateIdToMoveFrom = GetRealEstateByDeviceID(deviceToMoveID);
            DeviceProfile deviceToMove = Devices.FirstOrDefault(d => d.DeviceID == deviceToMoveID);
            RealEstate realEstateToMoveFrom = _currentUserWithData.RealEstates.First(r => r.RealEstateID == realEstateIdToMoveFrom);
            RealEstate realEstateToAddDevice = _currentUserWithData.RealEstates.First(r => r.RealEstateID == realEstateIdToAddDevice);
            realEstateToAddDevice.DevicesProfiles.Add(deviceToMove);
            realEstateToMoveFrom.DevicesProfiles.Remove(deviceToMove);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_currentUserWithData"></param>
        /// <param name="realEstateToAddDevice"></param>
        /// <param name="currentDevice"></param>
        /// <returns></returns>
        public async Task MoveDeviceFromUnassignedDevicesProfile(UserProfile _currentUserWithData, int realEstateToAddDevice, DeviceProfile currentDevice)
        {
            _currentUserWithData.RealEstates.First(r => r.RealEstateID == realEstateToAddDevice).DevicesProfiles.Add(currentDevice);
            _currentUserWithData.UnassignedDevicesList.Remove(currentDevice);
        }

        /// <summary>
        /// Getting item in the list by ID
        /// </summary>
        /// <param name="id">ID number</param>
        /// <returns>Device Profile from the list with matched device ID</returns>
        public DeviceProfile GetDeviceById(int id)
        {
            DeviceProfile currentDevice = new();
            int i = 0;
            DeviceProfile? currentDeviceTest = new DeviceProfile();
            DeviceProfile device;
            for (i = 0; i < _currentUserWithData.GetAllDevices().Count; i++)
            {
                device = _currentUserWithData.GetAllDevices()[i];
                if (device.DeviceID == id)
                {
                    currentDevice = device;
                    break;
                }
            }
            return currentDevice;
        }

        public async void Navigate(int deviceID, IJSRuntime jSRuntime)
        {
            DeviceProfile currentDevice = GetDeviceById(deviceID);
            var query = new Dictionary<string, string>
            {
            { $"{currentDevice.DeviceProduser}", $"{currentDevice.DeviceModelNumber}" }
        };
            string buildedUrl = Util.BuildUrlWithQueryStringUsingStringConcat(Program.Constants.BASE_API_URL, query);
            await jSRuntime.InvokeVoidAsync("open", buildedUrl, "_blank");
        }

        #endregion


        #region ShopDetails

        /// <summary>
        /// Adding new warranties profile to the shop
        /// </summary>
        /// <param name="shop">Shop profile to add warranties profile</param>
        public async Task AddShopInfo(Shop shop)
        {
            DeviceWarranty currentWarranty = CurrentDevice.DeviceWarranty;
            currentWarranty.Shop = shop;
            await _dbcontext.SaveChangesAsync();
        }

        /// <summary>
        /// Creating new Shop object and assigning adrress object to it
        /// </summary>
        /// <param name="address"></param>
        public async Task AddShopAdrress(Address address)
        {
            Shop shop = CurrentDevice.DeviceWarranty.Shop;
            shop.Address = address;
            await _dbcontext.SaveChangesAsync();
        }

        #endregion

        #region Warranties

        /// <summary>
        /// Method to get a device with closest expirig date to the actual date 
        /// </summary>
        /// <returns>Expiring device profile</returns>
        public DeviceProfile FirstExpiringWarranty()
        {
            List<DeviceProfile>? devicesList = _currentUserWithData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            //List<DeviceWarranty> warranties = DevicesWarranties;
            List<DeviceProfile> validWarrantiesList = new();
            DeviceProfile firstExpiringDevice = new();
            var counting = 0;
            DateTime date = DateTime.Now;

            foreach (DeviceProfile device in devicesList)
            {
                counting = date.CompareTo(device.DeviceWarranty.WarrantyEnd);
                if (counting < 0)
                {
                    validWarrantiesList.Add(device);
                }
            }
            if (validWarrantiesList.Count != 0)
            {
                var sortedList = validWarrantiesList.OrderBy(d => d.DeviceWarranty.WarrantyEnd);
                firstExpiringDevice = sortedList.First();
            }

            return firstExpiringDevice;
        }

        /// <summary>
        /// Method to update device warranty details 
        /// </summary>
        /// <param name="deviceWarranty">Chosed device warranty profile</param>
        /// <returns>Updating warranty details</returns>
        public async Task AddDeviceWarrantyInfo(DeviceWarranty deviceWarranty)
        {       // Do I need to take Years to database? How to awoid it ?   
            deviceWarranty.WarrantyPeriod = GetTimeSpanFromYears(deviceWarranty.Years);
            deviceWarranty.ExtraInsuranceWarrantyLenght = GetTimeSpanFromYears(deviceWarranty.ExtendedWarrantyinYears);
            CurrentDevice.DeviceWarranty = deviceWarranty;
            await _dbcontext.SaveChangesAsync();
        }

        
        public static TimeSpan GetTimeSpanFromYears(int years) // add days from editform 
        {
            int totalDaysInTheYear = 365;
            int yearsToDays = years * totalDaysInTheYear;
            TimeSpan interval = TimeSpan.FromDays(yearsToDays);
            string timeInterval = interval.ToString();
            int pIndex = timeInterval.IndexOf(':');
            pIndex = timeInterval.IndexOf('.', pIndex);
            if (pIndex < 0) timeInterval += "        ";

            Console.WriteLine("{0,21}{1,26}", yearsToDays, timeInterval);
            return interval;
        }
        #endregion

        #region Unassigned Devices
        /// <summary>
        /// Moves devices to a separate list where devices are not assigned to the real estate. This can be done later.
        /// </summary>
        /// <param name="currentRealEstate">The Real Estate to be Removed</param>
        public async void LeaveDevicesUnassigned(RealEstate currentRealEstate)
        {
            List<DeviceProfile> devicesToDelete = _currentUserWithData.RealEstates.First(r => r.RealEstateID == currentRealEstate.RealEstateID).DevicesProfiles;

            foreach (DeviceProfile deviceProfile in devicesToDelete.ToList())
            {
                _currentUserWithData.UnassignedDevicesList.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            await UpdateObjectInDB();
        }
        #endregion

        #region Should be moved?

        //  InputFile upploading handling // 
        /// <summary>
        /// Capturing file and creating filePath to return
        /// </summary>
        /// <param name="file">Loaded file in InputFile</param>
        /// <param name="maxFileSize">Limited file size</param>
        /// <param name="errors">List to add errors and later print them as needed</param>
        /// <returns>Created file path to the file in Blazor server</returns>
        public async Task<string> CaptureFilePath(IBrowserFile file, long maxFileSize, List<string> errors, DeviceProfile currentDevice)
        {
            if (file is null)
            {
                // returns empty string if there is no file
                return "";
            }

            try
            {
                string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.Name));
                string userId = _currentUserWithData.UserID.ToString();
                Directory.CreateDirectory($"{Environment.CurrentDirectory}\\files\\{userId}\\{currentDevice.DeviceID}");
                string filePath = $"{Environment.CurrentDirectory}\\files\\{userId}\\{currentDevice.DeviceID}\\{newFileName}";
                if (file.Size <= maxFileSize)
                {
                    using var content = new MultipartFormDataContent();
                    await using FileStream fs = new(filePath, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                    fs.Close();
                }
                else
                {
                    errors.Add($"File: {file.Name} Error: The File has exceed file size{maxFileSize}");
                }
                return filePath;
            }
            catch (Exception ex)
            {
                // TODO: for security reasons file.Name should be encoded or should remove all the special Characters and change the value for display
                errors.Add($"File: {file.Name} Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checking if there is a file assigned to the filepath. 
        /// If there is so it will be deleted to avoid saving multiple files in the server for the same object. 
        /// Old file always will be deleted and then added new to the server.
        /// </summary>
        /// <param name="filePath">Path to the file in the Blazor server</param>
        public void DeleteFileIfExists(string filePath)
        {
            if (!System.String.IsNullOrEmpty(filePath))
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// Checking if file exsists and if the link is active. If not so it will be changed to an empty string.
        /// </summary>
        public void CheckIfFileExsist()
        {
            // var files = Directory.GetFiles(Environment.CurrentDirectory + $"\\Files\\{DataService._currentUserWithData.UserID}", "*.*");
            List<DeviceProfile> deviceProfiles = _currentUserWithData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            foreach (DeviceProfile device in deviceProfiles)
            {
                if (!System.IO.File.Exists(device.DeviceWarranty.ReceiptLink))
                {
                    device.DeviceWarranty.ReceiptLink = "";
                }
                if (!System.IO.File.Exists(device.DeviceWarranty.ExtraInsuranceWarrantyLink))
                {
                    device.DeviceWarranty.ExtraInsuranceWarrantyLink = "";
                }
            }
        }

        public string GetFileUrl(string linkToTheFile, int deviceId)
        {
            var file = Path.GetFileName(linkToTheFile);
            string fileUrl = $"files/{_currentUserWithData.UserID}/{deviceId}/{file}";
            return fileUrl;
        }

        public async Task UpdateObjectInDB()
        {
            _dbcontext.UpdateRange(CurrentAppUser);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion


    }
}
