using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHome.Models
{
    public class Shop
    {
        private string _shopName = default;

        public string ShopName
        {
            get { return _shopName; }
            set { _shopName = value; }
        }

        private string _shopWebAdrress = default;

        public string ShopWebAddress
        {
            get { return _shopWebAdrress; }
            set { _shopWebAdrress = value; }
        }

        private Address _address;
        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private long _phoneNumber = 00000000;

        public long PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public override string ToString()
        {
            return _shopName + " " + _phoneNumber;
        }


    }
}
