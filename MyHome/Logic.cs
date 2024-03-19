using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
using QRCoder;
using MyHome.Models;
using System.IO;

namespace MyHome
{
    public class Logic
    {

        /// <summary>
        /// Method to get all devices with warranties which ending soon
        /// </summary>
        /// <param name="userProfile">Devices connected to Users</param>
        /// <param name="daysTillEnd">Max days until devices warranties ends</param>
        /// <returns>List of devices which are in the period of Max days to warraties ends</returns>
        public static List<DeviceProfile> ExpiringDevicesWarrantiesInDays(UserProfile userProfile, int daysTillEnd = 180)
        {
            List<DeviceProfile> expiringDevices = new List<DeviceProfile>();
            List<RealEstate> realEstates = userProfile.RealEstates;
            foreach (RealEstate realEstate in realEstates)
            {
                List<DeviceProfile> devices = realEstate.DevicesProfiles;
                foreach (DeviceProfile device in devices)
                {
                    string userDevice = device.DeviceName;
                    DeviceWarranty warranty = device.DeviceWarranty;
                    DateTime deviceWarranty = warranty.WarrantyEnd;
                    DateTime dateTime = DateTime.Now;
                    TimeSpan daysCounting = deviceWarranty.Subtract(dateTime);

                    if (daysCounting.Days < daysTillEnd)
                    {
                        expiringDevices.Add(device);
                    }
                }
            }
            expiringDevices = expiringDevices.OrderBy(d => d.DeviceWarranty.WarrantyEnd).ToList();
            return expiringDevices;
        }

        /// <summary>
        /// Geting all user's devices
        /// </summary>
        /// <param name="user"> Users </param>
        /// <returns>List of user devices</returns>
        public static List<DeviceProfile> GetAllUserDevices(UserProfile user)
        {
            List<DeviceProfile> allDevices = new List<DeviceProfile>();
            List<RealEstate> realEstates = user.RealEstates;
            var devices = realEstates.SelectMany(realEstate => realEstate.DevicesProfiles);
            foreach (DeviceProfile device in devices)
            {
                allDevices.Add(device);
            }
            return allDevices;
        }

        /// <summary>
        /// Geting user's devices expiring warranties dates
        /// </summary>
        /// <param name="user">Users</param>
        /// <returns>List of devices warranties</returns>
        public static List<DeviceWarranty> GetUserDevicesWarranties(UserProfile user)
        {
            List<DeviceWarranty> devicesWarranties = new List<DeviceWarranty>();
            List<RealEstate> realEstates = user.RealEstates;
            DeviceWarranty warranty = new DeviceWarranty();
            foreach (RealEstate realestate in realEstates)
            {
                List<DeviceProfile> devices = realestate.DevicesProfiles;
                foreach (DeviceProfile device in devices)
                {
                    warranty = device.DeviceWarranty;
                    devicesWarranties.Add(warranty);
                }
            }
            return devicesWarranties;
        }

        /// <summary>
        /// Method to move device object from one Real Estate to another Real Estatee by device's serial number
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="serialNumber">Device's serial number</param>
        /// <param name="userChosedRealEstateToMoveDevice">Chosed Real Estate where to move the device</param>
        /// <returns>bool to give a respons for user if device moving was successful </returns>
        public static bool MoveDeviceToOtherRealEstate(UserProfile user, int deviceID, RealEstate userChosedRealEstateToMoveDevice)
        {
            RealEstate targetRealEstate = userChosedRealEstateToMoveDevice;
            List<RealEstate> realEstateList = user.RealEstates;
            List<DeviceProfile> devices = new List<DeviceProfile>();
            DeviceProfile foundDevice = new();
            bool deviceWasAdded = false;

            foreach (RealEstate sourceRealestate in realEstateList)
            {
                devices = sourceRealestate.DevicesProfiles;
                foundDevice = devices.Where(d => d.DeviceID == deviceID).FirstOrDefault();
                break;
            }

            if (!targetRealEstate.DevicesProfiles.Contains(foundDevice))
            {
                userChosedRealEstateToMoveDevice.DevicesProfiles.Add(foundDevice);
                deviceWasAdded = true;
            }
            else
            {
                deviceWasAdded = false;
            }
            devices.Remove(foundDevice);
            return deviceWasAdded;
        }
        /// <summary>
        /// Method to create QrCode for devices 
        /// </summary>
        /// <param name="idNumber">Device's ID number</param>
        public static void CreateQRCodeForDevice(string idNumber)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"Device ID number: {idNumber}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
#pragma warning disable CA1416 // Validate platform compatibility
            qrCodeImage.Save("C:\\Users\\shiranco.DESKTOP-HRN41TE\\Desktop\\qrcodes\\qrCode.png");
#pragma warning restore CA1416 // Validate platform compatibility
        }

        /// <summary>
        /// Method to delete Device fromthe List
        /// </summary>
        /// <param name="deviceID">context Device ID</param>
        /// <param name="user">Current User</param>
        /// <returns>bool if it was removed true if not false</returns>
        public static void RemoveDevice(RealEstate currentRealEstate, DeviceProfile deviceToDelete, List<DeviceProfile> devices, List<UserProfile> _users, string _path)
        {
            currentRealEstate.DevicesProfiles.Remove(deviceToDelete);
            Data.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Method to get DeviceProfile by ID
        /// </summary>
        /// <param name="searchID">get device with this ID</param>
        /// <param name="currentUser">user profile</param>
        /// <returns>device profile</returns>
        public static DeviceProfile GetDeviceInfoByID(int searchID, UserProfile currentUser)
        {
            var foundDevice = currentUser.GetAllDevices().Where(d => d.DeviceID == searchID).FirstOrDefault();
            return foundDevice;
        }

        public static int GetUserMaxId(List<UserProfile> userProfiles)
        {
            int maxID = userProfiles.Max(u => u.UserID);
            return maxID;
        }

        public static int GetRealEstateMaxId(List<RealEstate> realEstates)
        {
            int maxID;
            if (realEstates == null)
            {
                maxID = 0;
            }
            else
            {
                maxID = realEstates.Max(rE => rE.RealEstateID);
            }
            return maxID;
        }

        public static int GetDeviceMaxId(List<DeviceProfile> devices)
        {
            int maxID;
            if (devices.Count == 0)
            {
                maxID = 1;
            }
            else
            {
                maxID = devices.Max(d => d.DeviceID);
            }
            return maxID;
        }



        /// <summary>
        /// Created two new lists and one dictionary to assign IDs and devices
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Dictionary</returns>
        //public static Dictionary<DeviceID, DeviceProfile> AddObjectsToDict(UserProfile user)
        //{
        //    List<RealEstate> realEstates = user.RealEstates;
        //    List<DeviceID> devicesIDs = new List<DeviceID>();
        //    Dictionary<List<DeviceID>, List<DeviceProfile>> dictObjectIdAndDevice = new Dictionary<List<DeviceID>, List<DeviceProfile>>();

        //    int indexStartingInDictionary = 1;
        //    List<DeviceProfile> devices = new List<DeviceProfile>();
        //    List<DeviceProfile> deviceProfiles = new List<DeviceProfile>();
        //    foreach (var realEstate in realEstates)
        //    {
        //        devices = realEstate.DevicesProfiles;
        //        foreach (var device in devices)
        //        {
        //            deviceProfiles.Add(device);
        //            DeviceID iD = new DeviceID();
        //            iD.ID = indexStartingInDictionary;
        //            devicesIDs.Add(iD);
        //            indexStartingInDictionary++;                   
        //        }
        ////    }
        //    // assigned index to device with zip method
        //    var dictIdAndDeviceProfile = devicesIDs.Zip(deviceProfiles).ToDictionary(x => x.First, x => x.Second);

        //    //trying to add lists of objects in to dictionary. Difficult to iterate
        //    dictObjectIdAndDevice.Add(devicesIDs, deviceProfiles);

        //    // returing dictionary 
        //    return dictIdAndDeviceProfile;
        //}
    }
}
