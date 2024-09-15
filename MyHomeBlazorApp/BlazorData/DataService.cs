using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Connections.Features;
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
        public RealEstate GetRealEstate(int realEstateID)
        {
            RealEstate rE = new RealEstate();
            if (RealEstates.Count > 1 && RealEstates.Any(item => item.RealEstateID == realEstateID))
            {
                rE = RealEstates.First(RealEstates => RealEstates.RealEstateID == realEstateID);
            }
            else
            {
                rE = new();
            }
            return rE;
        }

        public void AddNewRealEstate(RealEstate realEstate)
        {
            realEstate.RealEstateID = Logic.GetRealEstateMaxId(RealEstates) + 1;
            RealEstates.Add(realEstate);
            Data.SaveUsersListToXml(_users, _path);
        }

        public void AddRealEstateAdrress(Address adrress)
        {
            CurrentRealEstate.Address = adrress;
            Data.SaveUsersListToXml(_users, _path);
        }
        public int GetRealEstateByDeviceID(int deviceId)
        {
            RealEstate currentRealEstateTest = new();
            foreach (RealEstate r in RealEstates)
            {
                foreach (DeviceProfile d in r.DevicesProfiles)
                {
                    if (d.DeviceID == deviceId)
                    {
                        currentRealEstateTest = r;
                        break;
                    }
                }
                
            }
            return currentRealEstateTest.RealEstateID;
        }

        #endregion
        #region Device
        public static int GetDeviceMaxId(List<DeviceProfile> devices)
        {
            int maxID = devices.Max(d => d.DeviceID);
            return maxID;
        }
        public void AddNewDevice(DeviceProfile deviceToAdd, int chosedRealEstateID)
        {
            //validation code (duplicates etc)
            List<DeviceProfile> devicesList = Devices;
            int maxId = Logic.GetDeviceMaxId(devicesList);
            deviceToAdd.DeviceID = maxId+1;
            deviceToAdd.DeviceWarranty = new();
            deviceToAdd.DeviceWarranty.Shop = new();
            deviceToAdd.DeviceWarranty.Shop.Address = new();
            RealEstate? chosedRealEstate = RealEstates.FirstOrDefault(r => r.RealEstateID == chosedRealEstateID);
            
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
        public DeviceProfile GetDeviceById(int id)
        {
            DeviceProfile currentDevice = new DeviceProfile();
            currentDevice = CurrentUser.GetAllDevices().FirstOrDefault(d => d.DeviceID == id);
            //List<DeviceProfile> devicesList = CurrentUser.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            //foreach (DeviceProfile device in devicesList)
            //{
            //    if (device.DeviceID == id)
            //    {
            //        currentDevice = device;
            //        break;
            //    }
            //    else
            //    {
            //        currentDevice = new DeviceProfile();
            //    }
            //}
            //device = Devices.FirstOrDefault(Device => Device.DeviceID == id);
            return currentDevice;
        }

        public void AddDeviceWarrantyInfo(DeviceWarranty deviceWarranty)
        {       // Do I need to take Years to database? How to awoid it ?   
            deviceWarranty.WarrantyPeriod = GetTimeSpanFromYears(deviceWarranty.Years);
            deviceWarranty.ExtraInsuranceWarrantyLenght = GetTimeSpanFromYears(deviceWarranty.ExtendedWarrantyinYears);
            CurrentDevice.DeviceWarranty = deviceWarranty;
            Data.SaveUsersListToXml(_users, _path);
        }

        public void AddShopInfo(Shop shop)
        {
            DeviceWarranty currentWarranty = CurrentDevice.DeviceWarranty;
            currentWarranty.Shop = shop;
            Data.SaveUsersListToXml(_users, _path);
        }

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
            List<DeviceProfile>? devicesList = RealEstates.SelectMany(d => d.DevicesProfiles).ToList();
            //List<DeviceWarranty> warranties = DevicesWarranties;
            List<DeviceProfile> validWarrantiesList = new();
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

            var sortedList = validWarrantiesList.OrderBy(d => d.DeviceWarranty.WarrantyEnd);
            DeviceProfile firstExpiringDevice = sortedList.First();
            return firstExpiringDevice; // this shouldn't be marked
        }

        /// last added device should be matched by highest ID
        public DeviceProfile LastAddedDevice()
        {
            List<DeviceProfile>? devices = Devices;
            DeviceProfile? device = devices.Last();
            return device; // this shouldn't be marked
        }

        public RealEstate LastAddedRealEstate()
        {
            RealEstate lastRealEstate = RealEstates.Last();
            return lastRealEstate;
        }

        public void RemoveRealEstate(int contextChosedRealEstateID)
        {
            bool realEstateExsits = RealEstates.Contains(GetRealEstate(contextChosedRealEstateID));
            if (realEstateExsits == true)
            {
                RealEstate realEstateToDelete = RealEstates.First(r => r.RealEstateID == contextChosedRealEstateID);
                RealEstates.Remove(realEstateToDelete);
                Data.SaveUsersListToXml(_users, _path);
                _users = Data.GetUsersListFromXml(_path);
            }
            else
            {
                return;
            }
        }

        public void SaveUpdatedObject()
        {
            Data.SaveUsersListToXml(_users, _path);
        }

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
        /// <summary>
        /// Method to move devices list from one RealEstate to another
        /// </summary>
        /// <param name="realEstateID">RealEstate by ID to move</param>
        /// <param name="currentRealEstate">RealEstate to move from</param>
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
        /// method to move DeviceProfile from one Real Estate to Another
        /// </summary>
        /// <param name="deviceToMoveID">DeviceProfile by ID to change real estates</param>
        /// <param name="currentUser">Indentified user</param>
        /// <param name="realEstateIdToAddDevice">Real Estate ID to move DeviceProfile in</param>
        public void MoveDeviceToOtherRealEstate(int deviceToMoveID, UserProfile currentUser, int realEstateIdToAddDevice)
        {
            int realEstateIdToMoveFrom = GetRealEstateByDeviceID(deviceToMoveID);
            DeviceProfile deviceToMove = Devices.FirstOrDefault(d => d.DeviceID == deviceToMoveID);
            RealEstate realEstateToMoveFrom = currentUser.RealEstates.First(r => r.RealEstateID == realEstateIdToMoveFrom);
            RealEstate realEstateToAddDevice = currentUser.RealEstates.First(r => r.RealEstateID == realEstateIdToAddDevice);
            realEstateToAddDevice.DevicesProfiles.Add(deviceToMove);
            realEstateToMoveFrom.DevicesProfiles.Remove(deviceToMove);
            Data.SaveUsersListToXml(_users, _path);
        }

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

        //  InputFile upploading handling // 

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
            //
            // These do not get updated List and makes id duplicates ///
            //
            //List<DeviceProfile> _devices = currentUser.GetAllDevices();
            //Devices = Logic.GetAllUserDevices(CurrentUser);
            //Devices = CurrentUser.GetAllDevices();
            //Devices = CurrentUser.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            ///////////
            Device = LastAddedDevice();
            //UnassignedDevicesList = UnassignedDevices();
            UnassignedProfile = UnassignedDevices();
            CurrentDevice = GetDeviceById(deviceId);
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(CurrentUser, 180);
            DevicesWarranties = Logic.GetUserDevicesWarranties(CurrentUser);
            FirstExpiringDevice = FirstExpiringWarranty();
        }


        // I guess these should be as Parameters 
        private static readonly string _path = Program.Constants.XML_DATA_PATH;
        private static List<UserProfile>? _users;
        public List<UserProfile>? Users => _users;
        public string XmlPath => _path;

        // Why this get teh updates list but not in the class? Should I change all variables like this ?
        public List<DeviceProfile> Devices => CurrentUser.GetAllDevices();
        //

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
        public RealEstate DefaultRealEstate { get; set; } = new RealEstate();
    }
}
