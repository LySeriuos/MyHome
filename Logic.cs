using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List <DateTime> WarrantiesToTheList(UserProfile user)
        {
            List<DateTime> warranties = new List<DateTime>();
            List<RealEstate> realEstates = user.House;
            foreach (RealEstate house in realEstates)
            {
                List<DevicesProfile> devices = house.DevicesProfile;
                foreach (DevicesProfile device in devices)
                {
                    DeviceWarranty warranty = device.DeviceWarranty;
                    DateTime deviceWarranty = warranty.WarrantyEnd;
                    warranties.Add(warranty.WarrantyEnd);
                }
            }
            return warranties;
        }

    }
}
