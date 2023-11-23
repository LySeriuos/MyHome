using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class UserProfile
    {
        private int _userID;

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        // password class will be added later
        // not sure about this class here


        private List<RealEstate> _realEstate;

        public List<RealEstate> RealEstates
        {
            get { return _realEstate; }
            set { _realEstate = value; }
        }

        public List<DeviceProfile> GetAllDevices()
        {
            List<DeviceProfile> allDevices = new List<DeviceProfile>();
            var devices = RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles);
            foreach (DeviceProfile device in devices)
            {
                allDevices.Add(device);
            }
            return allDevices;
        }



        public override string ToString()
        {
            return _email + " " + _userName;
        }
    }
}
