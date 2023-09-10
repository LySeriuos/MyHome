using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Temp\usersListTestData.xml";
            List<UserProfile> usersList = TestData.User();
            Data.SaveQnAListToXml(usersList, path);
            
            List<DevicesProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], 180);
            foreach (DevicesProfile d in devicesClosestToTheEndList)
            {
                Console.WriteLine(d);
            }

            
            List<DevicesProfile> devices = Logic.GetAllUserDevices(usersList[1]);
            foreach (DevicesProfile device in devices)
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