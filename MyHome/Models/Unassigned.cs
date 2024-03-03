using MyHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Home.Models
{
    public class Unassigned
    {
        private string _unassignedDevice = "Unassigned";
        public string UnassignedDevice
        {
            get { return _unassignedDevice; }
            set { _unassignedDevice = value; }
        }

        private List<DeviceProfile> _unassignedDevicesList;

        public List<DeviceProfile> UnassignedDevicesList
        {
            get { return _unassignedDevicesList; }
            set { _unassignedDevicesList = value; }
        }

    }
}
