﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class RealEstate
    {
        [Required(ErrorMessage = "Please select Real Estate to Add Device......")]
        [Range (1, int.MaxValue)]
        private int _realEstateID;

        public int RealEstateID
        {
            get { return _realEstateID; }
            set { _realEstateID = value; }
        }

        [Required(ErrorMessage = "Please add name for your Real Estate.")]
        private string _realEstateName;
        [Required(ErrorMessage = "Please add name for your Real Estate.....")]
        public string RealEstateName
        {
            get { return _realEstateName; }
            set { _realEstateName = value; }
        }

        private Address _address = new();

        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private List<DeviceProfile> _devicesProfile = new();

        public List<DeviceProfile> DevicesProfiles
        {
            get { return _devicesProfile; }
            set { _devicesProfile = value; }
        }


        public override string ToString()
        {
            return _realEstateName;
        }


    }
}
