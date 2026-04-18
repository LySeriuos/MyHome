using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyHome.Models; // This fixes the "UserProfile/RealEstate not found" errors
using MyHome;        // This fixes the "Logic/XMLUtilities not found" errors

namespace My_Home
{
    public class ManualTester
    {
        public static void RunTests()
        {
            // 1. Setup Paths
            string baseFolder = AppContext.BaseDirectory;
            string path = @"C:\Temp\usersListTestData111.xml";
            string saveQRCodeLink = Path.Combine(baseFolder, "wwwroot", "images", "qrCodes", "qrCode.png");

            // 2. Run Logic (Ensuring we use the correct names from your project)
            const int DAYS_UNTILL_WARRANTY_ENDS = 180;

            // Note: I am assuming your utility class is spelled 'XMLFileUitilities' based on your error
            List<UserProfile> usersList = XMLFileUitilities.GetUsersListFromXml(path);

            if (usersList != null && usersList.Count > 1)
            {
                List<DeviceProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], DAYS_UNTILL_WARRANTY_ENDS);
                List<DeviceProfile> userDevices = Logic.GetAllUserDevices(usersList[1]);
                List<DeviceWarranty> devicesWarranties = Logic.GetUserDevicesWarranties(usersList[0]);

                RealEstate userChosedRealEstateToMoveDevice = usersList[1].RealEstates[0];
                int searchID = 6;
                bool movedDevice = Logic.MoveDeviceToOtherRealEstate(usersList[1], searchID, userChosedRealEstateToMoveDevice);

                RealEstate realEstate = usersList[1].RealEstates[1];
                UserProfile currentUser = usersList[1];
                List<RealEstate> realEstates = currentUser.RealEstates;

                int maxRealEId = realEstates.Max(r => r.RealEstateID);
                Console.WriteLine($"{maxRealEId}");

                DeviceProfile foundedDevice = Logic.GetDeviceInfoByID(searchID, currentUser);
                if (foundedDevice != null)
                {
                    Console.WriteLine(foundedDevice.ToString());
                }
            }
        }
    }
}