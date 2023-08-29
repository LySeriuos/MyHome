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
                        var leftToTheWarranty = deviceWarranty.WarrantyEnd - localDate;
                        Console.WriteLine(leftToTheWarranty.ToString("yyyy/MM/dd"));
                        expiringDate.Add(deviceName + " " + warrantyEndsToString);
                    }

                }
            }
            return expiringDate;
        }

        public static UserProfile AllTheUsers(UserProfile user)
        {
            return user;
        }

        public static void GetTheShortesWarranty(List<UserProfile> users)
        {

        }

    }
}
