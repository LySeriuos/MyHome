using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class UserProfile
    {
		private string _userName;

		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		private string _email;

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		// password class will be added later
		// not sure about this class here


		private List <RealEstate> _house;

		public List <RealEstate> House
		{
			get { return _house; }
			set { _house = value; }
		}

        public override string ToString()
        {
			return _email + " " + _userName;
        }
    }
}
