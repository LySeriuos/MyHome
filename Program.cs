using Aspose.Cells;
using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {
            // saved Test Data to local file
            string path = @"C:\Temp\usersListTestData.xml";
            List<UserProfile> usersList = TestData.Users();
            List<UserProfile> userProfiles = Data.GetUsersListFromXml(path);
            foreach (UserProfile userProfile in userProfiles)
            {             
                foreach(RealEstate realEstate in userProfile.RealEstates)
                {
                    Console.WriteLine(realEstate.RealEstateName);
                }
            }
            Data.SaveUsersListToXml(usersList, path);
            
            List<DeviceProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], 180);
            foreach (DeviceProfile d in devicesClosestToTheEndList)
            {
                Console.WriteLine(d);
            }

            
            List<DeviceProfile> devices = Logic.GetAllUserDevices(usersList[1]);
            foreach (DeviceProfile device in devices)
            {
                DeviceType deviceType = device.DeviceType;
                DeviceWarranty deviceWarranty = device.DeviceWarranty;
                Shop shop = deviceWarranty.Shop;
            }
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);



        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}