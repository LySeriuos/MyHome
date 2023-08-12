using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class DevicesProfile
    {
		private string _deviceName;

		public string DeviceName
		{
			get { return _deviceName; }
			set { _deviceName = value; }
		}

		private DeviceType _deviceType;

		public enum DeviceType
		{
			get { return _deviceType; }
			set { _deviceType = value; }
		}



	}
}
