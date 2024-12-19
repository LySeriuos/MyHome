using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Connections.Features;
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
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MyHomeBlazorApp.BlazorData

{

    public class DataService
    {
        #region User        

        // all the method to create a new user
        public void AddUser(UserProfile u)
        {
            _users.Add(u);
            Data.SaveUsersListToXml(_users, _path);
        }

        public int UserMaxID(List<UserProfile> _users)
        {
            int maxID = _users.Max(u => u.UserID);
            return maxID;
        }

        public UserProfile GetUser(int id)
        {
            List<UserProfile> u = _users;
            UserProfile uP = new UserProfile();
            uP = _users.First(u => u.UserID == id);
            return uP;
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
            if (RealEstates.Count > 1 && RealEstates.Any(item => item.RealEstateID == realEstateID))
            {
                realEstateById = RealEstates.First(RealEstates => RealEstates.RealEstateID == realEstateID);
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
        public void AddNewRealEstate(RealEstate realEstate)
        {
            realEstate.RealEstateID = Logic.GetRealEstateMaxId(RealEstates) + 1;
            RealEstates.Add(realEstate);
            Data.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Method to assign Adrress object to RealEstate 
        /// </summary>
        /// <param name="adrress">Newly created adress</param>
        public void AddRealEstateAdrress(Address adrress)
        {
            CurrentRealEstate.Address = adrress;
            Data.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Method to get RealEstate object by Device ID parameter
        /// </summary>
        /// <param name="deviceId">Parameter</param>
        /// <returns>RealEstate's ID</returns>
        public int GetRealEstateByDeviceID(int deviceId)
        {
            RealEstate currentRealEstateTest = new();
            foreach (RealEstate realEstate in RealEstates)
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
            RealEstate lastRealEstate = RealEstates.Last();
            return lastRealEstate;
        }

        /// <summary>
        /// Method to delete(remove) RealEstate from the RealEstates list
        /// </summary>
        /// <param name="contextChosedRealEstateID">Chosed RealEstate</param>
        public void RemoveRealEstate(int contextChosedRealEstateID)
        {
            bool realEstateExsits = RealEstates.Contains(GetRealEstate(contextChosedRealEstateID));
            if (realEstateExsits == true)
            {
                RealEstate realEstateToDelete = RealEstates.First(r => r.RealEstateID == contextChosedRealEstateID);
                RealEstates.Remove(realEstateToDelete);
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
        public static int GetDeviceMaxId(List<DeviceProfile> devices)
        {
            int maxID = devices.Max(d => d.DeviceID);
            return maxID;
        }

        /// <summary>
        /// Add new device to the devices list and assigning default values of nested classes
        /// </summary>
        /// <param name="deviceToAdd">Newly created device</param>
        /// <param name="chosedRealEstateID">Real Estate by ID to add new device profile</param>
        public void AddNewDevice(DeviceProfile deviceToAdd, int chosedRealEstateID)
        {
            //validation code (duplicates etc)
            List<DeviceProfile> devicesList = Devices;
            int maxId = Logic.GetDeviceMaxId(devicesList);
            deviceToAdd.DeviceID = maxId + 1;
            deviceToAdd.DeviceWarranty = new();
            deviceToAdd.DeviceWarranty.Shop = new();
            deviceToAdd.DeviceWarranty.Shop.Address = new();
            RealEstate chosedRealEstate = new();
            if (chosedRealEstateID != 0)
            {
                chosedRealEstate = RealEstates.First(r => r.RealEstateID == chosedRealEstateID);
            }

            if (Devices.Any(d => d.DeviceID == deviceToAdd.DeviceID))
            {
                return;
            }
            else
            {
                chosedRealEstate.DevicesProfiles.Add(deviceToAdd);
            }

            Data.SaveUsersListToXml(_users, _path);
            deviceToAdd = new();
        }

        /// <summary>
        /// Getting last item in the sequence
        /// </summary>
        /// <returns>Returns last added device in the list</returns>
        public DeviceProfile LastAddedDevice()
        {
            List<DeviceProfile>? devices = Devices;
            DeviceProfile? device = devices.Last();
            return device;
        }

        /// <summary>
        /// Method to move devices list from one RealEstate to another
        /// </summary>
        /// <param name="realEstateID">Devices list will be moved into RealEstate by realEstateID</param>
        /// <param name="currentRealEstate">RealEstate to move from(delete) devices list</param>
        public void MoveDeviceListToOtherRealEstate(int realEstateID, RealEstate currentRealEstate)
        {
            List<DeviceProfile> deviceProfilesMoveToRealEstate = CurrentUser.RealEstates.First(r => r.RealEstateID == realEstateID).DevicesProfiles;
            foreach (DeviceProfile deviceProfile in currentRealEstate.DevicesProfiles.ToList())
            {
                deviceProfilesMoveToRealEstate.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            Data.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Method to move DeviceProfile from one Real Estate to another
        /// </summary>
        /// <param name="deviceToMoveID">DeviceProfile ID which will be moved </param>
        /// <param name="currentUser">Indentified user</param>
        /// <param name="realEstateIdToAddDevice">Real Estate ID to move in Device by deviceToMoveID</param>
        public void MoveDeviceToOtherRealEstate(int deviceToMoveID, UserProfile currentUser, int realEstateIdToAddDevice)
        {
            int realEstateIdToMoveFrom = GetRealEstateByDeviceID(deviceToMoveID);
            DeviceProfile deviceToMove = Devices.FirstOrDefault(d => d.DeviceID == deviceToMoveID);
            RealEstate realEstateToMoveFrom = currentUser.RealEstates.First(r => r.RealEstateID == realEstateIdToMoveFrom);
            RealEstate realEstateToAddDevice = currentUser.RealEstates.First(r => r.RealEstateID == realEstateIdToAddDevice);
            realEstateToAddDevice.DevicesProfiles.Add(deviceToMove);
            realEstateToMoveFrom.DevicesProfiles.Remove(deviceToMove);
        }

        public void MoveDeviceFromUnassignedDevicesProfile(UserProfile currentUser, int realEstateToAddDevice, DeviceProfile currentDevice)
        {
            currentUser.RealEstates.First(r => r.RealEstateID == realEstateToAddDevice).DevicesProfiles.Add(currentDevice);
            currentUser.UnassignedDevices.UnassignedDevicesList.Remove(currentDevice);
        }

        /// <summary>
        /// Getting item in the list by ID
        /// </summary>
        /// <param name="id">ID number</param>
        /// <returns>Device Profile from the list with matched device ID</returns>
        public DeviceProfile GetDeviceById(int id)
        {
            int i = 0;
            DeviceProfile? currentDevice = new DeviceProfile();
            DeviceProfile device;
            for (i = 0; i < CurrentUser.GetAllDevices().Count; i++)
            {
                device = CurrentUser.GetAllDevices()[i];
                if (device.DeviceID == id)
                {
                    currentDevice = device;
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
        public void AddShopInfo(Shop shop)
        {
            DeviceWarranty currentWarranty = CurrentDevice.DeviceWarranty;
            currentWarranty.Shop = shop;
            Data.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Creating new Shop object and assigning adrress object to it
        /// </summary>
        /// <param name="address"></param>
        public void AddShopAdrress(Address address)
        {
            Shop shop = CurrentDevice.DeviceWarranty.Shop;
            shop.Address = address;
            Data.SaveUsersListToXml(_users, _path);
        }

        #endregion

        #region Warranties

        public DeviceProfile FirstExpiringWarranty()
        {
            List<DeviceProfile>? devicesList = CurrentUser.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
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
        public void AddDeviceWarrantyInfo(DeviceWarranty deviceWarranty)
        {       // Do I need to take Years to database? How to awoid it ?   
            deviceWarranty.WarrantyPeriod = GetTimeSpanFromYears(deviceWarranty.Years);
            deviceWarranty.ExtraInsuranceWarrantyLenght = GetTimeSpanFromYears(deviceWarranty.ExtendedWarrantyinYears);
            CurrentDevice.DeviceWarranty = deviceWarranty;
            Data.SaveUsersListToXml(_users, _path);
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
        public void LeaveDevicesUnassigned(RealEstate currentRealEstate)
        {
            List<DeviceProfile> devicesToDelete = CurrentUser.RealEstates.First(r => r.RealEstateID == currentRealEstate.RealEstateID).DevicesProfiles;

            foreach (DeviceProfile deviceProfile in devicesToDelete.ToList())
            {
                UnassignedProfile.UnassignedDevicesList.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            Data.SaveUsersListToXml(_users, _path);
        }

        public Unassigned UnassignedDevices()
        {
            if (CurrentUser.UnassignedDevices == null)
            {
                CurrentUser.UnassignedDevices = new();
                CurrentUser.UnassignedDevices.UnassignedDevicesList = new();
            }
            return CurrentUser.UnassignedDevices;
        }
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
                string userId = CurrentUser.UserID.ToString();
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
            // var files = Directory.GetFiles(Environment.CurrentDirectory + $"\\Files\\{DataService.CurrentUser.UserID}", "*.*");
            List<DeviceProfile> deviceProfiles = CurrentUser.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
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
            //currentDevice = GetDeviceById(deviceID);
            //string currentWarrantyReceiptLink = currentDevice.DeviceWarranty.ExtraInsuranceWarrantyLink;
            var file = Path.GetFileName(linkToTheFile);
            string fileUrl = $"files/{CurrentUser.UserID}/{file}";
            return fileUrl;
            //NavigationManager.NavigateTo(fileUrl, true);
            //await JSRuntime.InvokeVoidAsync("open", fileUrl, "_blank");
        }

        /// <summary>
        /// Saving new changes to xml database
        /// </summary>
        public void SaveUpdatedObject()
        {
            Data.SaveUsersListToXml(_users, _path);
        }

        public void LoadData()
        {
            Data.GetUsersListFromXml(_path);
        }

        #endregion

        public DataService()
        {
            _users = Data.GetUsersListFromXml(_path);
            // manually assigned test data
            int userId = 2;
            int realEstateID = 1;
            int deviceId = 2;
            CurrentUser = GetUser(userId);
            RealEstates = CurrentUser.RealEstates;
            CurrentRealEstate = GetRealEstate(realEstateID);
            Device = LastAddedDevice();
            UnassignedProfile = UnassignedDevices();
            CurrentDevice = GetDeviceById(deviceId);
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(CurrentUser, 180);
            DevicesWarranties = Logic.GetUserDevicesWarranties(CurrentUser);
        }


        // I guess these should be as Parameters 
        private static readonly string _path = Program.Constants.XML_DATA_PATH;
        private static List<UserProfile>? _users;
        public List<UserProfile>? Users => _users;
        public string XmlPath => _path;
        public List<DeviceProfile> Devices => CurrentUser.GetAllDevices();

        public List<DeviceProfile> ExpiringDevices { get; set; } = new List<DeviceProfile>();
        public DeviceProfile Device { get; set; } = new DeviceProfile();
        public UserProfile CurrentUser { get; set; } = new UserProfile();
        public RealEstate CurrentRealEstate { get; set; } = new RealEstate();
        public List<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
        public Address Adrress { get; set; } = new Address();
        public DeviceWarranty DeviceWarranty { get; set; } = new DeviceWarranty();
        public DeviceProfile FirstExpiringDevice { get; set; } = new DeviceProfile();
        //public List<DeviceProfile> UnassignedDevices { get; set; } = new List<DeviceProfile>();
        public List<DeviceWarranty> DevicesWarranties { get; set; } = new List<DeviceWarranty>();
        public DeviceProfile CurrentDevice { get; set; } = new DeviceProfile();
        public Shop Shop { get; set; } = new Shop();
        public Unassigned UnassignedProfile { get; set; } = new Unassigned();
        public List<DeviceProfile> UnassignedDevicesList { get; } = new List<DeviceProfile>();
    }
}
