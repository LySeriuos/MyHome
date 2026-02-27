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
        
        private int _realEstateID = new();
        [Key]
        public int RealEstateID
        {
            get { return _realEstateID; }
            set { _realEstateID = value; }
        }

        private string _realEstateName = string.Empty;
        [Required (ErrorMessage = "Add Real Estate Name")]
        //[StringLength(10, ErrorMessage = "Name is too long.")]
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
