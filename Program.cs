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
            foreach(var device in devices)
            {
                Console.WriteLine(device.DeviceID);
            }
            DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);


            // device selection by serial number for test purposes
            string deviceSerialNumber = "AKZ43CPQ505923D";
            RealEstate userChosedRealEstateToMoveDevice = usersList[1].RealEstates[0];

            bool goodAnswer = Logic.MoveDeviceToOtherRealEstate(usersList[1], deviceSerialNumber, userChosedRealEstateToMoveDevice);
            //Data.SaveUsersListToXml(usersList, path);
            // saved updated devices profile


            //example general adding new device 
            //var newDevice = UI.AddNewDevice();

            ////have user select realestate
            //RealEstate realEstate = new RealEstate();

            //UserProfile currentUser = new UserProfile();

            //List<DeviceProfile> devicesDetails = currentUser.GetAllDevices();

            //int maxID = devicesDetails.Max(d => d.ID);

            //newDevice.ID = maxID + 1;

            ////example id search

            //var searchID = 20;

            //var foundDevice = currentUser.GetAllDevices().Where(d=> d.ID == searchID).FirstOrDefault(); 


           // add new device to realestate



           // Logic.AddObjectsToDict(usersList[1]);

            
        }
        // public static object ExpiringFirstWarrantyList(UserProfile userProfile)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}