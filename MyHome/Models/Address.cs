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

        private int _apartamentNumber;

        public int ApartamentNumber
        {
            get { return _apartamentNumber; }
            set { _apartamentNumber = value; }
        }

        // """bla@1domain.tld
        private string _houseNumberExtension;
        [EmailAddress]
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

        public override string ToString()
        {
            return _streetName + " " + _city + "" + _houseNumber + "" + _country;
        }

    }
}
