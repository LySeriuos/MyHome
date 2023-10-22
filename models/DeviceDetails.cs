using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class DeviceDetails
    {

        public static List<string> GetDetails(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Audio:
                    return new List<string>() { "Speaker", "SoundBar", "Portable Speaker", "Amplifier" };

                case DeviceType.Video:
                    return new List<string>() { "TV", "Projector", "Monitor", "Camera", };

                case DeviceType.Computer:
                    return new List<string>() { "Computer", "Laptop", "Portable Hard Disk", "Tablet PC", "Home Assistant" };

                case DeviceType.MobileDevice:
                    return new List<string>() { "Phone", "Ipad", "Watch" };

                case DeviceType.Kitchen:
                    return new List<string>() { "Mixer", "Oven", "Microwave", "Airfryer", "Dish Washer" };

                case DeviceType.Bathroom:
                    return new List<string>() { "Hairdryer", "Shaving Machine", "Skin Lazer" };

                case DeviceType.Garden:
                    return new List<string>() { "Grass Trimmer", "Grass Cuting Robot", "Watering System" };

                case DeviceType.Cleaning:
                    return new List<string>() { "Washing Machine", "Drying Machine", "Robot Vacuum Cleaner" };

                case DeviceType.Security:
                    return new List<string>() { "Security Cameras", "Security Sensors" };

                case DeviceType.Other:
                    return new List<string>() { "Navigation", "Home Lighting" };

                case DeviceType.MultiDevice:
                    return new List<string>() { "Something special" };
                default:
                    return new List<string>() { "Not Sorted Device" };

            }

        }
    }
}

