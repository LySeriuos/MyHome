using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class GetEnumOfDeviceType
    {
        public static DeviceType SelectDeviceType(int Selection)
        {
            
            switch (Selection)
            {
                case 0:
                    return DeviceType.Audio;
                case 1:
                    return DeviceType.Computer;
                case 2:
                    return DeviceType.Kitchen;
                case 3:
                    return DeviceType.MobileDevice;
                case 4:
                    return DeviceType.Bathroom;
                case 5:
                    return DeviceType.Cleaning;
                case 6:
                    return DeviceType.Video;
                case 7:
                    return DeviceType.Garden;
                case 8:
                    return DeviceType.Security;
                case 9:
                    return DeviceType.MultiDevice;

                default:
                    return DeviceType.Other;
            }
        }
    }
}

