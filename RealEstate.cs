using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class RealEstate
    {
        private string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        private Address _propertyAdrress;

        public Address PropertyAdress
        {
            get { return _propertyAdrress; }
            set { _propertyAdrress = value; }
        }

        private List <DevicesProfile> _devicesProfile;

        public List <DevicesProfile> DevicesProfile
        {
            get { return _devicesProfile; }
            set { _devicesProfile = value; }
        }




    }
}
