using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home.models
{
    public enum DeviceType
    {
        Audio,
        Video,
        Computer,
        MobileDevice,
        Kitchen,
        Bathroom,
        Garden,
        Cleaning,
        Security,
        Other,
        MultiDevice, // these kind of devices has few purposes, but always one is the main so i don't know if I need this category.
    }

}
