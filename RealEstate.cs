using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class RealEstate
    {
        private int _realEstateID;

        public int RealEstateID
        {
            get { return _realEstateID; }
            set { _realEstateID = value; }
        }

        private string _realEstateName;

        public string RealEstateName
        {
            get { return _realEstateName; }
            set { _realEstateName = value; }
        }

        private Address _address;

        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private List <DeviceProfile> _devicesProfile;

        public List <DeviceProfile> DevicesProfiles
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
