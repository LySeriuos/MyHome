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
            List<DeviceProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], 180);
            List<DeviceProfile> devices = Logic.GetAllUserDevices(usersList[1]);            
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);

            // device selection by serial number for test purposes
            string deviceSerialNumber = "AKZ43CPQ505923D";
            RealEstate userChosedRealEstateToMoveDevice = usersList[1].RealEstates[0];
            
            bool goodAnswer = Logic.MoveDeviceToOtherRealEstate(usersList[1], deviceSerialNumber, userChosedRealEstateToMoveDevice);
            // saved updated devices profile
            Data.SaveUsersListToXml(usersList, path);
        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}