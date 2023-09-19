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
            string path = @"C:\Temp\usersListTestData111.xml";
            
            List<UserProfile> usersList = Data.GetUsersListFromXml(path);
            //Data.SaveUsersListToXml(usersList, path);
            
            List<DeviceProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], 180);  
            List<DeviceProfile> devices = Logic.GetAllUserDevices(usersList[1]);
           
            
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);
            List<DeviceProfile> device = Logic.MoveDeviceToOtherRealEstate(usersList[1]);
            foreach (DeviceProfile deviceProfile in device)
            {
                Console.WriteLine(deviceProfile.DeviceName + " " + deviceProfile.DeviceSerialNumber);
            }
        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}