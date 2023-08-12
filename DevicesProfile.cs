
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Device_Profile
{
	
    internal class DevicesProfile
    {
		
		private string _deviceName;
		public string DeviceName
		{
			get { return _deviceName; }
			set { _deviceName = value; }
		}

		private SoundDevicesType _soundDeviceType;
		public SoundDevicesType SoundDevicesType
		{
			get { return _soundDeviceType; }
			set { _soundDeviceType = value; }
		}

		private KitchenDevicesType _kitchenDevicesType;
		public KitchenDevicesType KitchenDevicesType
		{
			get { return _kitchenDevicesType; }
			set { _kitchenDevicesType = value; }
		}

		private WashingCleaningDevicesType _washingCleaningDevicesType;
		public WashingCleaningDevicesType WashingCleaningDevicesType
		{
			get { return _washingCleaningDevicesType; }
			set { _washingCleaningDevicesType = value; }
		}

		private VisualDevicesType _visualDeviceType;
		public VisualDevicesType VisualDevicesType
		{
			get { return _visualDeviceType; }
			set { _visualDeviceType = value; }
		}

		private SmartDevicesType _smartDevicesType;
		public SmartDevicesType SmartDevicesType
		{
			get { return _smartDevicesType; }
			set { _smartDevicesType = value; }
		}

		private string _deviceModelNumber;

		public string DeviceModelNumber
		{
			get { return _deviceModelNumber; }
			set { _deviceModelNumber = value; }
		}

		private string myVar;

		public string MyProperty
		{
			get { return myVar; }
			set { myVar = value; }
		}









	}
}
