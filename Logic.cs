using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using IronBarCode;
using System.Security.Cryptography;

namespace My_Home
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
            expiringDevices = expiringDevices.OrderByDescending(d => d.DeviceWarranty.WarrantyEnd).ToList();
            return expiringDevices;
        }

        /// <summary>
        /// Geting all user's devices
        /// </summary>
        /// <param name="user"> Users </param>
        /// <returns>List of user devices</returns>
        public static List<DeviceProfile> GetAllUserDevices(UserProfile user)
        {
            List<DeviceProfile> devices = new List<DeviceProfile>();
            List<RealEstate> house = user.RealEstates;
            foreach (RealEstate realestate in house)
            {
                devices = realestate.DevicesProfiles;
            }
            return devices;
        }

        /// <summary>
        /// Geting user's devices expiring warranties dates
        /// </summary>
        /// <param name="user">Users</param>
        /// <returns>List of devices warranties</returns>
        public static DeviceWarranty GetUserDevicesWarranties(UserProfile user)
        {
            List<RealEstate> house = user.RealEstates;
            DeviceWarranty warranty = new DeviceWarranty();
            foreach (RealEstate realestate in house)
            {
                List<DeviceProfile> devices = realestate.DevicesProfiles;
                foreach (DeviceProfile device in devices)
                {
                    warranty = device.DeviceWarranty;
                }
            }
            return warranty;
        }

        /// <summary>
        /// Method to move device object from one Real Estate to another Real Estatee by device's serial number
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="serialNumber">Device's serial number</param>
        /// <param name="userChosedRealEstateToMoveDevice">Chosed Real Estate where to move the device</param>
        /// <returns>bool to give a respons for user if device moving was successful </returns>
        public static bool MoveDeviceToOtherRealEstate(UserProfile user, string serialNumber, RealEstate userChosedRealEstateToMoveDevice)
        {
            RealEstate targetRealEstate = userChosedRealEstateToMoveDevice;
            Console.WriteLine(targetRealEstate);
            List<RealEstate> realEstateList = user.RealEstates;
            List<DeviceProfile> devices = new List<DeviceProfile>();
            DeviceProfile foundDevice = new();
            bool deviceWasAdded = false;

            foreach (RealEstate sourceRealestate in realEstateList)
            {
                devices = sourceRealestate.DevicesProfiles;
                foundDevice = devices.Where(d => d.DeviceSerialNumber == serialNumber).FirstOrDefault();
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

        public static void CreateQRCodeForEveryDevice(UserProfile user)
        {
            List<RealEstate> realEstates = user.RealEstates;
            var devices = from realEstate in realEstates
                          from DeviceDetails device in realEstate.DevicesProfiles
                          select device;
            QRCodeWriter.CreateQrCode($"" +
                $"" +
                $"'", 500, QRCodeWriter.QrErrorCorrectionLevel.Medium).SaveAsPng("MyQR.png");
        }

        /// <summary>
        /// Created two new lists and one dictionary to assign IDs and devices
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<DeviceID, DeviceProfile> AddObjectsToDict(UserProfile user)
        {
            List<RealEstate> realEstates = user.RealEstates;
            List<DeviceID> devicesIDs = new List<DeviceID>();
            Dictionary<List<DeviceID>, List<DeviceProfile>> dictObjectIdAndDevice = new Dictionary<List<DeviceID>, List<DeviceProfile>>();

            int indexStartingInDictionary = 1;
            List<DeviceProfile> devices = new List<DeviceProfile>();
            List<DeviceProfile> deviceProfiles = new List<DeviceProfile>();
            foreach (var realEstate in realEstates)
            {
                devices = realEstate.DevicesProfiles;
                foreach (var device in devices)
                {
                    deviceProfiles.Add(device);
                    DeviceID iD = new DeviceID();
                    iD.ID = indexStartingInDictionary;
                    devicesIDs.Add(iD);
                    indexStartingInDictionary++;                   
                }
            }
            // assigned index to device with zip method
            var dictIdAndDeviceProfile = devicesIDs.Zip(deviceProfiles).ToDictionary(x => x.First, x => x.Second);

            //trying to add lists of objects in to dictionary. Difficult to iterate
            dictObjectIdAndDevice.Add(devicesIDs, deviceProfiles);
           
            // returing dictionary 
            return dictIdAndDeviceProfile;
        }
    }
}
