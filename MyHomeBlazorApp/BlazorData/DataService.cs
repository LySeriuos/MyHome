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


namespace MyHomeBlazorApp.BlazorData

{

    public class DataService
    {
        // this is for fast tests 
        #region User
        private static readonly string _path = @"C:\Temp\usersListTestData111.xml";

        private List<UserProfile> _users;

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
            if (RealEstates.Count > 1)
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
        public void AddNewDevice(DeviceProfile device, int chosedRealEstateID)
        {
            //validation code (duplicates etc)
         
                
                //DeviceWarranty currentWarranty = CurrentDevice.DeviceWarranty;
                //currentWarranty.Shop = shop;
                //Data.SaveUsersListToXml(_users, _path);

                //Shop shop = CurrentDevice.DeviceWarranty.Shop;
                //shop.Address = address;
                Address address = new Address();
                device.DeviceID = CurrentUser.GetAllDevices().Max(d => d.DeviceID) + 1;
                //device.DeviceID = Logic.GetDeviceMaxId(Devices) + 1;
                device.DeviceWarranty = new();
                device.DeviceWarranty.Shop = new();
                device.DeviceWarranty.Shop.Address = address;
                //shopDetails.Address = new Address();
                
                RealEstate realEstateToAddDevice = GetRealEstate(chosedRealEstateID);
                realEstateToAddDevice.DevicesProfiles.Add(device);
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
        public DeviceProfile GetDeviceById(int id)
        {            
            DeviceProfile currentDevice = new DeviceProfile();
            List<DeviceProfile> devicesList = CurrentUser.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            foreach(DeviceProfile device in devicesList)
            {
                if(device.DeviceID == id)
                {
                    currentDevice = device;
                    break;
                }
                else
                {
                    currentDevice = new DeviceProfile();
                }
            }
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
            List<DeviceProfile>? devices = Devices;
            DeviceProfile? expiringDevice = new();
            bool deviceIsNull = devices.Where(device => device.DeviceWarranty == null).Any();
            if (deviceIsNull == true)
            {
                expiringDevice = new();
            }
            else
            {
                //expiringDevice = devices.OrderBy(d => d.DeviceWarranty.WarrantyEnd).FirstOrDefault();
            }
            return expiringDevice; // this shouldn't be marked
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
            RealEstate realEstateToDelete = RealEstates.First(r => r.RealEstateID == contextChosedRealEstateID);
            RealEstates.Remove(realEstateToDelete);
            Data.SaveUsersListToXml(_users, _path);
            _users = Data.GetUsersListFromXml(_path);
        }

        public void SaveUpdatedObject()
        {         
            Data.SaveUsersListToXml(_users, _path);
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
        /// Main Idea is to create default Real Estate and use it to add unnasigned devices.
        /// If real estate has ID = 0, show text = "unassigned" instead of ID number.
        /// Do not use any values except ID
        /// Need to work through methods MoveDevices and LeaveDevicesUnassigned to be more flexible.
        /// </summary>
        /// <param name="currentRealEstate">The Real Estate to be Removed</param>
        public void LeaveDevicesUnassigned(RealEstate currentRealEstate)
        {
            List<DeviceProfile> devicesToDelete = CurrentUser.RealEstates.First(r => r.RealEstateID == currentRealEstate.RealEstateID).DevicesProfiles;

            //if (RealEstates.Any(r => r.RealEstateID == 0) == false)
            //{
            //    DefaultRealEstate.RealEstateID = 0;
            //    DefaultRealEstate.RealEstateName = "Unassigned";
            //    DefaultRealEstate.DevicesProfiles = new List<DeviceProfile>();
            //    RealEstates.Add(DefaultRealEstate);
            //}
            //DefaultRealEstate.DevicesProfiles = devicesToDelete;


            foreach (DeviceProfile deviceProfile in devicesToDelete.ToList())
            {
                UnassignedProfile.UnassignedDevicesList.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            CurrentUser.RealEstates.Remove(currentRealEstate);

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
        public DataService()
        {
            _users = Data.GetUsersListFromXml(_path);
            // is it good practise to do like this?
            Users = _users;
            XmlPath = _path;
            // manually assigned test data
            int userId = 2;
            int realEstateID = 1;
            int deviceId = 2;
            CurrentUser = GetUser(userId);
            RealEstates = CurrentUser.RealEstates;
            CurrentRealEstate = GetRealEstate(realEstateID);
            //List<DeviceProfile> _devices = currentUser.GetAllDevices();
            Devices = CurrentUser.GetAllDevices();
            Device = LastAddedDevice();
            //UnassignedDevicesList = UnassignedDevices();
            UnassignedProfile = UnassignedDevices();
            CurrentDevice = GetDeviceById(deviceId);
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(CurrentUser, 180);
            DevicesWarranties = Logic.GetUserDevicesWarranties(CurrentUser);
            FirstExpiringDevice = FirstExpiringWarranty();
        }

        [Required(ErrorMessage = "This field is Required")]
        // I guess these should be as Parameters 

        public List<UserProfile> Users { get; set; } = new List<UserProfile>();
        public string XmlPath { get; set; } = string.Empty;
        public List<DeviceProfile> Devices { get; set; } = new List<DeviceProfile>();
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
