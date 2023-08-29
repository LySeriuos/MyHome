using MyHome;
using System.Collections.Generic;

namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            List<UserProfile> list = TestData.User();
            
            
            List <string> warrantiesList = Logic.ExpiringFirstWarrantyList(list);

            foreach (string item in warrantiesList)
            {
                Console.WriteLine(item);
            }

            //var x = GetDevicesCloseToWarrantyEnd(list[0]);


        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}