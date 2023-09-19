using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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

        public static List<DeviceProfile> MoveDeviceToOtherRealEstate(UserProfile user, string serialNumber, DeviceProfile selectedDevice)
        {            
            List<RealEstate> realEstateList = user.RealEstates;
            
            List<DeviceProfile> devicesListInRealEstate1 = realEstateList[0].DevicesProfiles;
            List<DeviceProfile> devicesInRealEstate2 = realEstateList[1].DevicesProfiles;
            DeviceProfile userDevice = devicesListInRealEstate1[2];
            //Console.WriteLine(userDevice.DeviceName);
            foreach (RealEstate realEstate in realEstateList)
            {
                List<DeviceProfile> devices = realEstate.DevicesProfiles;
                bool objectExistInList = devices.Contains(selectedDevice);
                Console.WriteLine(objectExistInList);
                if (objectExistInList)
                {

                }
                
                foreach (DeviceProfile device in devices)
                {                    
                    if (device.DeviceSerialNumber == serialNumber)
                    {
                        int objectIndexInList = devices.IndexOf(device);
                        //Console.WriteLine(objectIndexInList);
                        int realEstateObjectIndex = realEstateList.IndexOf(realEstate);
                        //Console.WriteLine(realEstateObjectIndex);
                        devicesListInRealEstate1.Add(device);
                        devices.RemoveAt(objectIndexInList);
                        break;
                    }
                }
            }
            return devicesListInRealEstate1;
        }
    }

}
