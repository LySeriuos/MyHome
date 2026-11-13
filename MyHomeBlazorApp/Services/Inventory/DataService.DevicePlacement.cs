using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using My_Home;
using MyHome;
using MyHome.Models;
using System.Xml;
//using System.Data.Entity;

namespace MyHomeBlazorApp.BlazorData
{
    public partial class DataService
    {
        /// <summary>
        /// Moves devices to a separate list where devices are not assigned to the real estate. This can be done later.
        /// </summary>
        /// <param name="currentRealEstate">The Real Estate to be Removed</param>
        public async Task LeaveDevicesUnassigned(RealEstate currentRealEstate)
        {
            RealEstate realEstateInDB = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == currentRealEstate.RealEstateID);
            if (realEstateInDB == null) return;

            List<DeviceProfile> devicesToMove = realEstateInDB.DevicesProfiles.ToList();
            foreach (DeviceProfile deviceProfile in devicesToMove)
            {
                _currentUserWithAllData.UnassignedDevicesList.Add(deviceProfile);
                realEstateInDB.DevicesProfiles.Remove(deviceProfile);
            }
            await UpdateObjectInDB();
        }
    }
}
