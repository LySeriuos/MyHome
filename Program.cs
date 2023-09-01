using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            List<UserProfile> list = TestData.User();
            List<string> devicesClosestToTheEndList = Logic.DevicesAndWarranties(list[0]);
            foreach (string d in devicesClosestToTheEndList)
            {
                Console.WriteLine(d);
            }

            //var x = GetDevicesCloseToWarrantyEnd(list[0]);


        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}