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
foreach (var device in userDevices)
{
    Console.WriteLine(device.DeviceID);
}
DeviceWarranty warranty = Logic.GetUserDevicesWarranties(usersList[0]);


// device selection by serial number for test purposes
string deviceSerialNumber = "AKZ43CPQ505923D";
RealEstate userChosedRealEstateToMoveDevice = usersList[1].RealEstates[0];

bool goodAnswer = Logic.MoveDeviceToOtherRealEstate(usersList[1], deviceSerialNumber, userChosedRealEstateToMoveDevice);
//var newDevice = UI.AddNewDevice();

// variables for testing
RealEstate realEstate = usersList[1].RealEstates[1];
UserProfile currentUser = usersList[1];

//// Adding new ID for new Device
//List<DeviceProfile> devicesDetails = currentUser.GetAllDevices();
//int maxID = devicesDetails.Max(d => d.DeviceID);
//Console.WriteLine($"{maxID} good");
//newDevice.DeviceID = maxID + 1;

//// adding new device to selected real Estate
//realEstate.DevicesProfiles.Add(newDevice);



string idNumber = userDevices[0].DeviceID.ToString();
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode($"Device ID number: {idNumber}", QRCodeGenerator.ECCLevel.Q);
QRCode qrCode = new QRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(20);
#pragma warning disable CA1416 // Validate platform compatibility
qrCodeImage.Save("C:\\Users\\shiranco.DESKTOP-HRN41TE\\Desktop\\qrcodes\\qrCode.png");
#pragma warning restore CA1416 // Validate platform compatibility
                              // saved updated userDevices profile
//Data.SaveUsersListToXml(usersList, path);
