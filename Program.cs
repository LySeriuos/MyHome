using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            List<UserProfile> list = TestData.User();
            UserProfile users = Logic.AllTheUsers(list[0]);
            List<string> devicesWarranties = Logic.GetTheShortesWarranty(list[1]);
            foreach (string device in devicesWarranties)
            {
                //Console.WriteLine(device);

            }


            List <string> warrantiesList = Logic.ExpiringFirstWarrantyList(list);

            

            //var x = GetDevicesCloseToWarrantyEnd(list[0]);


        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}