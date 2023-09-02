using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class Logic
    {
        public static List<string> ExpiringFirstWarrantyList(List<UserProfile> users)
        {
            List<string> expiringDate = new List<string>();
            List<RealEstate> houses;
            List<DevicesProfile> devices;
            DateTime localDate = DateTime.Now;

            for (int i = 0; i < users.Count; i++)
            {
                houses = users[i].House;

                for (int j = 0; j < houses.Count; j++)
                {
                    devices = houses[j].DevicesProfile;
                    for (int k = 0; k < devices.Count; k++)
                    {
                        string deviceName = devices[k].DeviceName;
                        DeviceWarranty deviceWarranty = devices[k].DeviceWarranty;
                        string warrantyEndsToString = deviceWarranty.WarrantyEnd.ToString("yyyy/MM/d");
                        expiringDate.Add(deviceName + " " + warrantyEndsToString);
                    }
                }
            }
            return expiringDate;
        }

        public static List<string> DevicesAndWarranties(UserProfile userProfile)
        {
            List<string> devicesWarranties = new List<string>();
            List<RealEstate> realEstates = userProfile.House;
            foreach (RealEstate house in realEstates)
            {
                List<DevicesProfile> devices = house.DevicesProfile;
                foreach (DevicesProfile device in devices)
                {                    
                    string houseName = house.RealEstateName;
                    string userDevice = device.DeviceName;
                    DeviceWarranty warranty = device.DeviceWarranty;
                    DateTime deviceWarranty = warranty.WarrantyEnd;
                    DateTime dateTime = DateTime.Now;
                    TimeSpan daysCounting = deviceWarranty.Subtract(dateTime);

                    var totalDaysWarrantyEnds = 0;
                    string convertingToDays = daysCounting.ToString("%d");
                    try
                    {
                        totalDaysWarrantyEnds = Int32.Parse(convertingToDays);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    // if warranties end earlier than 5 month before the date so it will be printed to the user
                    if (totalDaysWarrantyEnds < 180)
                    {
                        devicesWarranties.Add($"{userDevice} in {houseName} Warranty is Expiring in: {convertingToDays} days");                        
                    }
                }
            }
            devicesWarranties = devicesWarranties.OrderByDescending(convertingToDays => convertingToDays).ToList();
            return devicesWarranties;
        }
 
        public static List<RealEstate> RealEstates(UserProfile user)
        {
            List<RealEstate> house = user.House;            
            return house;
        }

        public static List<DevicesProfile> GetAllUserDevices(UserProfile user)
        {
            List<DevicesProfile> devices = new List<DevicesProfile>();
            List<RealEstate> house = user.House;
            foreach(RealEstate realestate in house)
            {
                devices = realestate.DevicesProfile;
            }
            return devices;
        }

        public static DeviceWarranty GetUserDevicesWarranties(UserProfile user)
        {             
            List<RealEstate> house = user.House;
            DeviceWarranty warranty = new DeviceWarranty();
            foreach (RealEstate realestate in house)
            {
                List<DevicesProfile> devices = realestate.DevicesProfile;
                foreach(DevicesProfile device in devices)
                {
                    warranty = device.DeviceWarranty;
                }                
            }
            return warranty;
        }








    }

}
