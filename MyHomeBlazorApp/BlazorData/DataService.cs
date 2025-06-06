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
using MyHome.Models;
using MyHomeBlazorApp.Pages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using Unity;
using static com.sun.management.VMOption;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(_currentUserWithData, 180);
            DevicesWarranties = Logic.GetUserDevicesWarranties(_currentUserWithData);
        }

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private UserManager<MyHomeBlazorAppUser> _userManager;
        private MyHomeBlazorAppContext _dbcontext;
        private UserProfile _currentUserWithData;
        public UserProfile CurrentUserWithAllData => _currentUserWithData;
        public MyHomeBlazorAppUser CurrentAppUser;
        private static readonly string _path = Program.Constants.XML_DATA_PATH;
        private static List<UserProfile>? _users;
        public List<UserProfile>? Users => _users;
        public string XmlPath => _path;
        public List<DeviceProfile>? Devices => _currentUserWithData.GetAllDevices();
        public List<DeviceProfile>? ExpiringDevices { get; set; } = new List<DeviceProfile>();
        public DeviceProfile? Device { get; set; } = new DeviceProfile();
        public RealEstate? CurrentRealEstate { get; set; } = new RealEstate();
        //   public List<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
        public Address? Adrress { get; set; } = new Address();
        public DeviceWarranty? DeviceWarranty { get; set; } = new DeviceWarranty();
        public DeviceProfile? FirstExpiringDevice { get; set; } = new DeviceProfile();
        //public List<DeviceProfile> UnassignedDevices { get; set; } = new List<DeviceProfile>();
        public List<DeviceWarranty>? DevicesWarranties { get; set; } = new List<DeviceWarranty>();
        public DeviceProfile? CurrentDevice { get; set; } = new DeviceProfile();
        public Shop? Shop { get; set; } = new Shop();
        public List<DeviceProfile>? UnassignedDevicesList { get; set; }

        #region User        

        // all the method to create a new user
        //public void AddUser(UserProfile u)
        //{
        //    _users.Add(u);
        //    Data.SaveUsersListToXml(_users, _path);
        //}

        //public int UserMaxID(List<UserProfile> _users)
        //{
        //    int maxID = _users.Max(u => u.UserID);
        //    return maxID;
        //}

        //public UserProfile GetUser(int id)
        //{
        //    List<UserProfile> u = _users;
        //    UserProfile uP = new UserProfile();
        //    uP = _users.First(u => u.UserID == id);
        //    return uP;
        //}

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

        public async Task<UserProfile> GetDbUserData()
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
            if (_currentUserWithData.RealEstates.Count > 1 && _currentUserWithData.RealEstates.Any(item => item.RealEstateID == realEstateID))
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
        //public void AddNewRealEstateToXML(RealEstate realEstate)
        //{
        //    realEstate.RealEstateID = Logic.GetRealEstateMaxId(_currentUserWithData.RealEstates) + 1;
        //    _currentUserWithData.RealEstates.Add(realEstate);
        //    Data.SaveUsersListToXml(_users, _path);
        //}

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
        /// Method to assign Adrress object to RealEstate 
        /// </summary>
        /// <param name="adrress">Newly created adress</param>
        //public void AddRealEstateAdrress(Address adrress)
        //{
        //    CurrentRealEstate.Address = adrress;
        //    Data.SaveUsersListToXml(_users, _path);
        //}

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
        public void RemoveRealEstate(int contextChosedRealEstateID)
        {
            bool realEstateExsits = _currentUserWithData.RealEstates.Contains(GetRealEstate(contextChosedRealEstateID));
            if (realEstateExsits == true)
            {
                RealEstate realEstateToDelete = _currentUserWithData.RealEstates.First(r => r.RealEstateID == contextChosedRealEstateID);
                _currentUserWithData.RealEstates.Remove(realEstateToDelete);
                Data.SaveUsersListToXml(_users, _path);
            }
            else
            {
                return;
            }
        }

        #endregion
        #region Devices

        /// <summary>
        /// Getting Max Id number of the devices in the list.
        /// </summary>
        /// <param name="devices">List of devices</param>
        /// <returns>highest ID int</returns>
        //public static int GetDeviceMaxId(List<DeviceProfile> devices)
        //{
        //    int maxID = devices.Max(d => d.DeviceID);
        //    return maxID;
        //}

        /// <summary>
        /// Add new device to the devices list and assigning default values of nested classes
        /// </summary>
        /// <param name="deviceToAdd">Newly created device</param>
        /// <param name="chosedRealEstateID">Real Estate by ID to add new device profile</param>
        //public void AddNewDevice(DeviceProfile deviceToAdd, int chosedRealEstateID)
        //{
        //    //validation code (duplicates etc)
        //    List<DeviceProfile> devicesList = Devices;
        //    int maxId = Logic.GetDeviceMaxId(devicesList);
        //    deviceToAdd.DeviceID = maxId + 1;
        //    deviceToAdd.DeviceWarranty = new();
        //    deviceToAdd.DeviceWarranty.Shop = new();
        //    deviceToAdd.DeviceWarranty.Shop.Address = new();
        //    RealEstate chosedRealEstate = new();
        //    if (chosedRealEstateID != 0)
        //    {
        //        chosedRealEstate = _currentUserWithData.RealEstates.First(r => r.RealEstateID == chosedRealEstateID);
        //    }

        //    if (Devices.Any(d => d.DeviceID == deviceToAdd.DeviceID))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        chosedRealEstate.DevicesProfiles.Add(deviceToAdd);
        //    }

        //    Data.SaveUsersListToXml(_users, _path);
        //    deviceToAdd = new();
        //}

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
            //foreach(DeviceProfile device in _currentUserWithData.GetAllDevices())
            //{
            //    if(device.DeviceID == id)
            //    {
            //        return device;
            //    }
            //    currentDevice = device;
            //}
            //return currentDevice;
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

        public string GetExpiringDevice()
        {
            string firstDevice = "";
            if (Device.DeviceWarranty == null)
            {
                Device.DeviceWarranty = new();
            }
            else
            {
                DeviceProfile expiringWarranty = FirstExpiringDevice;
                string deviceName = expiringWarranty.DeviceName;
                string deviceExpiringWarranty = expiringWarranty.DeviceWarranty.WarrantyEnd.ToShortDateString();
                firstDevice = deviceName + " " + deviceExpiringWarranty;

            }
            return firstDevice;
        }
        #endregion

        public void OnModalDeviceNext(DeviceProfile currentDevice)
        {
            int currentDeviceIndex = 0;
            List<DeviceProfile> allDevices = Devices;
            if (currentDevice != null)
            {
                currentDeviceIndex = allDevices.IndexOf(currentDevice);
                currentDevice = allDevices[currentDeviceIndex + 1];
                //one line code with linq
                // currentDevice = allDevices[(allDevices.IndexOf(currentDevice) + 1) % allDevices.Count];            
            }
        }

        #region Unassigned Devices
        /// <summary>
        /// Main Idea is to create default Real Estate and use it to add unnasigned devices.
        /// If real estate has ID = 0, show text = "unassigned" instead of ID number.
        /// Do not use any values except ID
        /// Need to work through methods MoveDevices and LeaveDevicesUnassigned to be more flexible.
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
           await _dbcontext.SaveChangesAsync();
        }

        //public Unassigned UnassignedDevices()
        //{
        //    if (_currentUserWithData.UnassignedDevices == null)
        //    {
        //        _currentUserWithData.UnassignedDevices = new();
        //        _currentUserWithData.UnassignedDevices.UnassignedDevicesList = new();
        //    }
        //    return _currentUserWithData.UnassignedDevices;
        //}
        #endregion

        #region Should be moved?
        // these should be moved somwhere else
        //  InputFile upploading handling // 
        /// <summary>
        /// Capturing file and creating filePath to return
        /// </summary>
        /// <param name="file">Loaded file in InputFile</param>
        /// <param name="maxFileSize">Limited file size</param>
        /// <param name="errors">List to add errors and later print them as needed</param>
        /// <returns>Created file path to the file in Blazor server</returns>
        public async Task<string> CaptureFilePath(IBrowserFile file, long maxFileSize, List<string> errors)
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
                Directory.CreateDirectory($"{Environment.CurrentDirectory}\\files\\{userId}");
                string filePath = $"{Environment.CurrentDirectory}\\files\\{userId}\\{newFileName}";
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

        public string GetFileUrl(string linkToTheFile)
        {
            var file = Path.GetFileName(linkToTheFile);
            string fileUrl = $"files/{_currentUserWithData.UserID}/{file}";
            return fileUrl;
        }

        /// <summary>
        /// Saving new changes to xml database
        /// </summary>
        //public void SaveUpdatedObject()
        //{

        //    Data.SaveUsersListToXml(_users, _path);
        //}

        public async Task UpdateObjectInDB()
        {           
            _dbcontext.UpdateRange(CurrentAppUser);
            await _dbcontext.SaveChangesAsync();
        }

        //public void LoadData()
        //{
        //    Data.GetUsersListFromXml(_path);
        //}

        #endregion


    }
}
