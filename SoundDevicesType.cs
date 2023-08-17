using My_Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device_Profile
{
    public static class DeviceDetails
    {
        public static List<string> GetAudio()
        {
            var list = new List<string>() { "Stereo Cable", "bla" };

            return list;
        }

        public static List<string> GetDetail(DeviceType d)
        {
            switch (d)
            {
                case DeviceType.Audio:
                    var list = new List<string>() { "Stereo Cable", "bla" };
                    break;

            }

            return list;
        }
        //StereoBluetooth,
        //EarphonesCabel,
        //EarphonesBluetooth,
        //Radio,
        //SpeakerBluetooth,
        //SpeakerCable,
        //Other}


        //}
        //StereoCable,
        //StereoBluetooth,
        //EarphonesCabel,
        //EarphonesBluetooth,
        //Radio,
        //SpeakerBluetooth,
        //SpeakerCable,
        //Other
    }
}
