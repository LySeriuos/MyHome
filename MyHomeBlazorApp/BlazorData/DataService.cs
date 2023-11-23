using Microsoft.AspNetCore.Connections.Features;
using MyHome;
using MyHome.Models;
using MyHomeBlazorApp.Pages;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        public RealEstate GetRealEstate(int id)
        {
            RealEstate rE = new RealEstate();
            rE = RealEstates.First(RealEstates => RealEstates.RealEstateID == id);
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

        #endregion
        #region Device
        public static int GetDeviceMaxId(List<DeviceProfile> devices)
        {
            int maxID = devices.Max(d => d.DeviceID);
            return maxID;
        }
        public bool AddNewDevice(DeviceProfile device)
        {
            //validation code (duplicates etc)
            if (true)
            {
                device.DeviceID = Logic.GetDeviceMaxId(Devices) + 1;
                CurrentRealEstate.DevicesProfiles.Add(device);
                Data.SaveUsersListToXml(_users, _path);
                return true;
            }
            else
                return false;
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
            DeviceProfile device = new DeviceProfile();
            device = Devices.First(Device => Device.DeviceID == id);
            return device;
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
        public DataService()
        {
            _users = Data.GetUsersListFromXml(_path);
            // manually assigned test data
            int userId = 1;
            int realEstateID = 1;
            int deviceId = 2;
            CurrentUser = GetUser(userId);
            RealEstates = CurrentUser.RealEstates;
            CurrentRealEstate = GetRealEstate(realEstateID);
            //List<DeviceProfile> _devices = currentUser.GetAllDevices();            
            Devices = Logic.GetAllUserDevices(CurrentUser);
            CurrentDevice = GetDeviceById(deviceId);
            ExpiringDevices = Logic.ExpiringDevicesWarrantiesInDays(CurrentUser, 180);
            DevicesWarranties = Logic.GetUserDevicesWarranties(CurrentUser);
        }





        [Required(ErrorMessage = "This field is Required")]
        // I guess these should be as Parameters 

        public List<UserProfile> Users { get; set; } = new List<UserProfile>();
        public List<DeviceProfile> Devices { get; set; } = new List<DeviceProfile>();
        public List<DeviceProfile> ExpiringDevices { get; set; } = new List<DeviceProfile>();
        public DeviceProfile Device { get; set; } = new DeviceProfile();
        public UserProfile CurrentUser { get; set; } = new UserProfile();
        public RealEstate CurrentRealEstate { get; set; } = new RealEstate();
        public List<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
        public Address Adrress { get; set; } = new Address();
        public DeviceWarranty DeviceWarranty { get; set; } = new DeviceWarranty();
        public List<DeviceWarranty> DevicesWarranties { get; set; } = new List<DeviceWarranty>();
        public DeviceProfile CurrentDevice { get; set; } = new DeviceProfile();
        public Shop Shop { get; set; } = new Shop();

    }
}
