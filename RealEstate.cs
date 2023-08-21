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

        private List <DevicesProfile> _devicesProfile;

        public List <DevicesProfile> DevicesProfile
        {
            get { return _devicesProfile; }
            set { _devicesProfile = value; }
        }




    }
}
