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
            foreach (UserProfile user in users)
            {
                List<RealEstate> houses = user.House;
                foreach (RealEstate house in houses)
                {
                    List<DevicesProfile> devices = house.DevicesProfile;
                    foreach (DevicesProfile device in devices)
                    {                         
                        string deviceName  = device.DeviceName; 
                        DeviceWarranty deviceWarranty = device.DeviceWarranty;
                        DateTime dateTime = deviceWarranty.WarrantyEnd;
                        Console.WriteLine(dateTime);


                        string dateTimeString = dateTime.ToString("yyyy/MM/d");
                        string warrantyEndsToString = deviceWarranty.WarrantyEnd.ToString("yyyy/MM/d");
                        expiringDate.Add( deviceName);
                        
                        continue;
                    }
                    continue;
                }
                
            }
            return expiringDate;
        }

    }
}
