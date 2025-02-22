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
        public int Id { get; set; }
        private List<DeviceProfile> _unassignedDevicesList = new();

        public List<DeviceProfile> UnassignedDevicesList
        {
            get { return _unassignedDevicesList; }
            set { _unassignedDevicesList = value; }
        }

    }
}
