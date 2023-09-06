using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UserProfile> list = TestData.User();
            List<DevicesProfile> devicesClosestToTheEndList = Logic.DevicesAndWarranties(list[1], 180);
            foreach (DevicesProfile d in devicesClosestToTheEndList)
            {
                Console.WriteLine(d);
            }

            DeviceType deviceType;
            List<DevicesProfile> devices = Logic.GetAllUserDevices(list[1]);
            foreach (DevicesProfile device in devices)
            {
                deviceType = device.DeviceType;
                DeviceWarranty deviceWarranty = device.DeviceWarranty;
                Shop shop = deviceWarranty.Shop;
            }
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(list[0]);



        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}