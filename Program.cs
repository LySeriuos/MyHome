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

            List<RealEstate> realEstate = Logic.RealEstates(list[0]);
            List<DevicesProfile> devices = Logic.GetAllUserDevices(list[0]);
            foreach (DevicesProfile device in devices)
            {
                Console.WriteLine(device.DeviceSerialNumber + " " + device.DeviceName);
            }

            

            //var x = GetDevicesCloseToWarrantyEnd(list[0]);


        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}