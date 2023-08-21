
using My_Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyHome
{
	
    public class DevicesProfile
    {
		
		private string _deviceName;
		public string DeviceName
		{
			get { return _deviceName; }
			set { _deviceName = value; }
		}

		private DeviceType _deviceType;
		public DeviceType DeviceType
		{
			get { return _deviceType; }
			set { _deviceType = value; }
		}

		private string _deviceModelNumber;

		public string DeviceModelNumber
		{
			get { return _deviceModelNumber; }
			set { _deviceModelNumber = value; }
		}

		private string _deviceSerialNumber;	

		public string DeviceSerialNumber
		{
			get { return _deviceSerialNumber; }
			set { _deviceSerialNumber = value; }
		}


		private string _ipAdrress;

		public string IpAdress
		{
			get { return _ipAdrress; }
			set { _ipAdrress = value; }
		}

		private string _macAdrress;

		public string MacAdrress
		{
			get { return _macAdrress; }
			set { _macAdrress = value; }
		}

		private string _deviceProduser;

		public string DeviceProduser
		{
			get { return _deviceProduser; }
			set { _deviceProduser = value; }
		}

		private string _manualBookLink;

		public string ManualBookLink
		{
			get { return _manualBookLink; }
			set { _manualBookLink = value; }
		}

		private DeviceWarranty _deviceWarranty;

		public DeviceWarranty DeviceWarranty
        {
			get { return _deviceWarranty; }
			set { _deviceWarranty = value; }
		}















	}
}
