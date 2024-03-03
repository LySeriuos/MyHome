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
        private string _unassignedDevicesStatus = "Unassigned";
        public string UnassignedDeviceStatus
        {
            get { return _unassignedDevicesStatus; }
            set { _unassignedDevicesStatus = value; }
        }

        private List<DeviceProfile> _unassignedDevicesList;

        public List<DeviceProfile> UnassignedDevicesList
        {
            get { return _unassignedDevicesList; }
            set { _unassignedDevicesList = value; }
        }

    }
}
