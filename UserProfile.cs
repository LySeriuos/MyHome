using Device_Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class UserProfile
    {
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
		private List<DevicesProfile> _devicesProfile;
		
		public List<DevicesProfile> DevicesProfile
		{
			get { return _devicesProfile; }
			set { _devicesProfile = value; }
		}

		private List <Property> _property;

		public List <Property> Property
		{
			get { return _property; }
			set { _property = value; }
		}


	}
}
