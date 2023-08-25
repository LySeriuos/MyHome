using My_Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome
{
    public static class DeviceDetails
    {
        public static List<string> GetAudio()
        {
            var audioList = new List<string>() { "Speaker", "SoundBar", "Portable Speaker", "Amplifier" };

            return audioList;
        }

        public static List<string> GetVideo()
        {
            var videoList = new List<string>() { "TV", "Projector", "Monitor", "Camera", };

            return videoList;
        }

        public static List<string> GetComputer()
        {
            var computerList = new List<string>() { "Computer", "Laptop", "Portable Hard Disk", "Tablet PC", "Home Assistant" };

            return computerList;
        }

        public static List<string> GetMobileDevice()
        {
            var mobileDeviceList = new List<string>() { "Phone", "Ipad", "Watch" };

            return mobileDeviceList;
        }

        public static List<string> GetKitchen()
        {
            var kitchenList = new List<string>() { "Mixer", "Oven", "Microwave", "Airfryer", "Dish Washer" };

            return kitchenList;
        }

        public static List<string> GetBathroom()
        {
            var bathroomList = new List<string>() { "Hairdryer", "Shaving Machine", "Skin Lazer" };

            return bathroomList;
        }

        public static List<string> GetGarden()
        {
            var gardenList = new List<string>() { "Grass Trimmer", "Grass Cuting Robot", "Watering System" };

            return gardenList;
        }

        public static List<string> GetCleaning()
        {
            var cleaningList = new List<string>() { "Washing Machine", "Drying Machine", "Robot Vacuum Cleaner" };

            return cleaningList;
        }

        public static List<string> GetSecurity()
        {
            var securityList = new List<string>() { "Security Cameras", "Security Sensors" };

            return securityList;
        }

        public static List<string> GetOtherDevices()
        {
            var otherDevicesList = new List<string>() { "Navigation", "Home Lighting" };

            return otherDevicesList;
        }

        public static List<string> GetMultiDevices()
        {
            var multiDevicesList = new List<string>() { "Something special" };

            return multiDevicesList;
        }

        //private static List<String> GetDetails(DeviceType deviceType)
        //{
        //    switch (deviceType)
        //    {
        //        case DeviceType.Audio:
        //            GetAudio();
        //            break;

        //        case DeviceType.Video:
        //            GetVideo();
        //            break;

        //        case DeviceType.Computer:
        //            GetComputer();
        //            break;

        //        case DeviceType.MobileDevice:
        //            GetMobileDevice();
        //            break;

        //        case DeviceType.Kitchen:
        //            GetKitchen();
        //            break;

        //        case DeviceType.Bathroom:
        //            GetBathroom();
        //            break;

        //        case DeviceType.Garden:
        //            GetGarden();
        //            break;

        //        case DeviceType.Cleaning:
        //            GetCleaning();
        //            break;

        //        case DeviceType.Security:
        //            GetSecurity();
        //            break;

        //        case DeviceType.Other:
        //            return new List<string>() { "Navigation", "Home Lighting" };
        //            break;

        //        case DeviceType.MultiDevice:
        //            GetMultiDevices();
        //            break;

        //    }
        //}
    }
}

