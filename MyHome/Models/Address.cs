using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class Address
    {
        private string _streetName = "No Data";

        public string StreetName
        {
            get { return _streetName; }
            set { _streetName = value; }
        }

        private int _houseNumber = 0;

        public int HouseNumber
        {
            get { return _houseNumber; }
            set { _houseNumber = value; }
        }

        private int _apartamentNumber = 0;

        public int ApartamentNumber
        {
            get { return _apartamentNumber; }
            set { _apartamentNumber = value; }
        }

        // """bla@1domain.tld
        private string _houseNumberExtension = "No Data";
        [EmailAddress]
        public string HouseNumberExtension
        {
            get { return _houseNumberExtension; }
            set { _houseNumberExtension = value; }
        }

        private string _city = "No Data";

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _country = "No Data";

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public override string ToString()
        {
            return _streetName + " " + _city + "" + _houseNumber + "" + _country;
        }

    }
}
