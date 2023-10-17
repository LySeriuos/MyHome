using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home.models
{
    public class Shop
    {
        private string _shopName;

        public string ShopName
        {
            get { return _shopName; }
            set { _shopName = value; }
        }

        private string _shopWebAdrress;

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

        private long _phoneNumber;

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
