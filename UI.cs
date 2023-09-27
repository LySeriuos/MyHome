using Aspose.Cells;
using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class UI
    {

        public DeviceProfile AddNewDevice(UserProfile user)
        {
            DeviceProfile deviceProfile = new DeviceProfile();
            RealEstate chosedRealEstate = new RealEstate();
            

            Console.WriteLine("Add new Device Name");
            chosedRealEstate.DevicesProfiles.Add(new DeviceProfile
            {
                DeviceName = Console.ReadLine(),
                //DeviceType = ,
                DeviceModelNumber = Console.ReadLine(),
                DeviceSerialNumber = Console.ReadLine(),
                IpAddress = Console.ReadLine(),
                MacAdrress = Console.ReadLine(),
                DeviceProduser = Console.ReadLine(),
                ManualBookLink = Console.ReadLine(),
            }); 
            
            //user.RealEstates = Console.ReadLine();
            return deviceProfile;
        }
    }
}
