using Aspose.Cells;
using MyHome;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using My_Home;


// saved Test Data to local file
string path = @"C:\Temp\usersListTestData111.xml";

List<UserProfile> usersList = Data.GetUsersListFromXml(path);
List<DeviceProfile> devicesClosestToTheEndList = Logic.ExpiringDevicesWarrantiesInDays(usersList[1], 180);
List<DeviceProfile> userDevices = Logic.GetAllUserDevices(usersList[1]);

DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);


// device selection by serial number for test purposes

RealEstate userChosedRealEstateToMoveDevice = usersList[1].RealEstates[0];
int searchID = 6;
bool movedDevice = Logic.MoveDeviceToOtherRealEstate(usersList[1], searchID, userChosedRealEstateToMoveDevice);

// variables for testing
RealEstate realEstate = usersList[1].RealEstates[1];
UserProfile currentUser = usersList[1];


// Adding new ID for new Device
//var newDevice = UI.AddNewDevice();
//List<DeviceProfile> devicesDetails = currentUser.GetAllDevices();
//int maxID = devicesDetails.Max(d => d.DeviceID);
//Console.WriteLine($"{maxID} good");
//newDevice.DeviceID = maxID + 1;

//// adding new device to selected real Estate
//realEstate.DevicesProfiles.Add(newDevice);

// getting device ID from Qr Code

DeviceProfile foundedDevice = Logic.GetDeviceInfoByID(searchID, currentUser);
Console.WriteLine(foundedDevice.ToString());//creating Qr code for device 
string idNumber = userDevices[0].DeviceID.ToString();
Logic.CreateQRCodeForDevice(idNumber);
// saved updated userDevices profile
//Data.SaveUsersListToXml(usersList, path);
