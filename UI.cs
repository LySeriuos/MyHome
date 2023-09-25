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

        public static RealEstateToChoose SelectRealEstate()
        {
            Console.WriteLine("Please make your Real Estate selection\n");
            Console.WriteLine("0 Real Estate 1");
            Console.WriteLine("1 Real Estate 2");
            int Selection = int.Parse(Console.ReadLine());
            switch (Selection)
            {
                case 0:
                    return RealEstateToChoose.RealEstate1;
                case 1:
                    return RealEstateToChoose.RealEstate2;
                default:
                    return RealEstateToChoose.INVALID;
            }
        }

        public DeviceProfile AddNewDevice(UserProfile user, RealEstateToChoose selection)
        {
            DeviceProfile deviceProfile = new DeviceProfile();
            RealEstate chosedRealEstate = new RealEstate();
            if (selection == RealEstateToChoose.RealEstate1)
            {
                chosedRealEstate = user.RealEstates[0];

            }
            else if (selection == RealEstateToChoose.RealEstate2)
            {
                chosedRealEstate = user.RealEstates[1];
            }
            else
            {
                Console.WriteLine("Some Error");
            };

            Console.WriteLine("Add new Device Name");
            chosedRealEstate.DevicesProfiles.Add(new DeviceProfile
            {
                DeviceName = Console.ReadLine(),
            });
            //user.RealEstates = Console.ReadLine();
            return deviceProfile;
        }
    }
}
