using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class RealEstate
    {
        [Required(ErrorMessage = "Please select your country......")]
        private int _realEstateID;

        [Required(ErrorMessage = "Please select your country.")]
        public int RealEstateID
        {
            get { return _realEstateID; }
            set { _realEstateID = value; }
        }

        [Required(ErrorMessage = "Please select your country.")]
        private string _realEstateName;
        [Required(ErrorMessage = "Please select your country....")]
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
