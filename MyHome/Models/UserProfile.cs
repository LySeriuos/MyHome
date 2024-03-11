using My_Home.Models;
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
               

        private List<RealEstate> _realEstates;

        public List<RealEstate> RealEstates
        {
            get { return _realEstates; }
            set { _realEstates = value; }
        }

        public List<DeviceProfile> GetAllDevices()
        {
            List<DeviceProfile> allDevices = new List<DeviceProfile>();
            var devices = RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles);
            var unassignedDevices = UnassignedDevices.UnassignedDevicesList;
            foreach (DeviceProfile device in devices)
            {
                allDevices.Add(device);
            }
            if(unassignedDevices != null)
            {
                foreach(DeviceProfile device in unassignedDevices)
                {
                    allDevices.Add(device);
                }
            }
            return allDevices.OrderBy(id => id.DeviceID).ToList();
        }

        private Unassigned _unassignedDevices;

        public Unassigned UnassignedDevices
        {
            get { return _unassignedDevices; }
            set { _unassignedDevices = value; }
        }

        public override string ToString()
        {
            return _email + " " + _userName;
        }
    }
}
