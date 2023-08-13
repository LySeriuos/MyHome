using Device_Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    internal class Property
    {
        private string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        private PropertyAdrress _propertyAdrress;

        public PropertyAdrress PropertyAdress
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
