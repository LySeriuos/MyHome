using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class UserProfile
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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


		private List <RealEstate> _realEstate;

		public List <RealEstate> RealEstates
		{
			get { return _realEstate; }
			set { _realEstate = value; }
		}

		private List <DeviceID> _deviceID;
		public List	<DeviceID> DevicesIDs
		{
			get { return _deviceID; }
			set { _deviceID = value; }
		}

        //private Dictionary<DeviceID, DeviceProfile> _devicesIdAndObjectDevices;

        //public Dictionary<DeviceID, DeviceProfile> DevicesIdAndObjectDevices
        //{
        //	get { return _devicesIdAndObjectDevices; }
        //	set { _devicesIdAndObjectDevices = value; }
        //}

        public List<DeviceProfile> GetAllDevices()
		{
			throw new NotImplementedException();
		}



        public override string ToString()
        {
			return _email + " " + _userName;
        }
    }
}
