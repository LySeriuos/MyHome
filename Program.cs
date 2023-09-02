using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            List<UserProfile> list = TestData.User();
            List<string> devicesClosestToTheEndList = Logic.DevicesAndWarranties(list[1]);
            //foreach (string d in devicesClosestToTheEndList)
            //{
            //    Console.WriteLine(d);
            //}

            List<RealEstate> realEstates = Logic.RealEstates(list[1]);
            
            List<DevicesProfile> devices = Logic.GetAllUserDevices(list[1]);
            foreach (DevicesProfile device in devices)
            {
                DeviceWarranty deviceWarranty = device.DeviceWarranty;
                Shop shop = deviceWarranty.Shop;


                Console.WriteLine(shop.ShopWebAddress.ToString());
            }
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(list[0]);
            



        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}