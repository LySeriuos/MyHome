﻿using MyHome;
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
        /// Geting all users' devices
        /// </summary>
        /// <param name="users"> Users </param>
        /// <returns>List of all devices</returns>
        public static List<string> ExpiringFirstWarrantyList(List<UserProfile> users)
        {
            List<string> expiringDate = new List<string>();
            List<RealEstate> houses;
            List<DevicesProfile> devices;
            DateTime localDate = DateTime.Now;

            for (int i = 0; i < users.Count; i++)
            {
                houses = users[i].RealEstates;

                for (int j = 0; j < houses.Count; j++)
                {
                    devices = houses[j].DevicesProfiles;
                    for (int k = 0; k < devices.Count; k++)
                    {
                        string deviceName = devices[k].DeviceName;
                        DeviceWarranty deviceWarranty = devices[k].DeviceWarranty;
                        string warrantyEndsToString = deviceWarranty.WarrantyEnd.ToString("yyyy/MM/d");
                        expiringDate.Add(deviceName + " " + warrantyEndsToString);
                    }
                }
            }
            return expiringDate;
        }
        /// <summary>
        /// Method to get all devices with warranties which ending soon
        /// </summary>
        /// <param name="userProfile">Devices connected to User</param>
        /// <param name="daysTillEnd">Max days until devices warranties ends</param>
        /// <returns>List of devices which are in the period of Max days to warraties ends</returns>
        public static List<DevicesProfile> ExpiringDevicesWarrantiesInDays(UserProfile userProfile, int daysTillEnd = 180)
        {
            List<DevicesProfile> expiringDevices = new List<DevicesProfile>();
            List<RealEstate> realEstates = userProfile.RealEstates;
            foreach (RealEstate realEstate in realEstates)
            {
                List<DevicesProfile> devices = realEstate.DevicesProfiles;
                foreach (DevicesProfile device in devices)
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
        /// <param name="user"> User </param>
        /// <returns>List of user devices</returns>
        public static List<DevicesProfile> GetAllUserDevices(UserProfile user)
        {
            List<DevicesProfile> devices = new List<DevicesProfile>();
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
        /// <param name="user">User</param>
        /// <returns>List of devices warranties</returns>
        public static DeviceWarranty GetUserDevicesWarranties(UserProfile user)
        {
            List<RealEstate> house = user.RealEstates;
            DeviceWarranty warranty = new DeviceWarranty();
            foreach (RealEstate realestate in house)
            {
                List<DevicesProfile> devices = realestate.DevicesProfiles;
                foreach (DevicesProfile device in devices)
                {
                    warranty = device.DeviceWarranty;
                }
            }
            return warranty;
        }








    }

}
