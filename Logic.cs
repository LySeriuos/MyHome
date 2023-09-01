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
                    string convertingToDays = daysCounting.ToString("%d");
                    devicesWarranties.Add($"{userDevice} in {houseName} Warranty Expiring in: {convertingToDays} days");
                }
            }
            devicesWarranties = devicesWarranties.OrderByDescending(convertingToDays => convertingToDays).ToList();
            return devicesWarranties;

        }


    }

}
