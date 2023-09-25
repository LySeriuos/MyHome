using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public class DeviceID
    {
		private int _iD = 0;
		public int ID
		{
			get { return _iD; }
			set { _iD = value; }
		}
        public override string ToString()
        {
            return _iD.ToString();
        }

    }
}
