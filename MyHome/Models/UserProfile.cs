using My_Home.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class UserProfile
    {
        private int _userID;
        [Key]
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

        private List<RealEstate> _realEstates = new();

        public List<RealEstate> RealEstates
        {
            get { return _realEstates; }
            set { _realEstates = value; }
        }

        

        private List<DeviceProfile> _unassignedDevicesList = new();

        public List<DeviceProfile> UnassignedDevicesList
        {
            get { return _unassignedDevicesList; }
            set { _unassignedDevicesList = value; }
        }

        public List<DeviceProfile> GetAllDevices()
        {
            List<DeviceProfile> allDevices = new List<DeviceProfile>();
            var devices = RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles);
            if(UnassignedDevicesList != null )
            {
                foreach (DeviceProfile device in UnassignedDevicesList)
                {
                    allDevices.Add(device);
                }
            }
            
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
