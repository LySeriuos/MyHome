using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            List<UserProfile> list = TestData.User();
            List<DevicesProfile> devicesClosestToTheEndList = Logic.DevicesAndWarranties(list[1], 1500);
            foreach (DevicesProfile d in devicesClosestToTheEndList)
            {
                Console.WriteLine(d);
            }

            List<RealEstate> realEstates = Logic.RealEstates(list[1]);

            DeviceType deviceType;
            List<DevicesProfile> devices = Logic.GetAllUserDevices(list[1]);
            foreach (DevicesProfile device in devices)
            {
                deviceType = device.DeviceType;
                DeviceWarranty deviceWarranty = device.DeviceWarranty;
                Shop shop = deviceWarranty.Shop;
                //Console.WriteLine(shop.ShopWebAddress.ToString());
            }
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(list[0]);
            List<DeviceType> deviceDetails = Logic.GetSubcategoryList(devices);


        //    List<string> strings1 = DeviceDetails.GetDetails(deviceType);



        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}