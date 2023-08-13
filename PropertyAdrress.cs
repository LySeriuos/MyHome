﻿using Device_Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class PropertyAdrress
    {
		private string _streetName;

		public string StreetName
		{
			get { return _streetName; }
			set { _streetName = value; }
		}

		private int _houseNumber;

		public int HouseNumber
		{
			get { return _houseNumber; }
			set { _houseNumber = value; }
		}

		private string _houseNumberExtension;

		public string HouseNumberExtension
		{
			get { return _houseNumberExtension; }
			set { _houseNumberExtension = value; }
		}

		private string _city;

		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		private string _country;

		public string Country
		{
			get { return _country; }
			set { _country = value; }
		}

		private DevicesProfile _devicesProfile;

		public DevicesProfile DevicesProfile
		{
			get { return _devicesProfile; }
			set { _devicesProfile = value; }
		}

	}
}
