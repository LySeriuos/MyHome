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
        public void AddToPrintQueue(DeviceProfile selectedDevice)
        {
            if (!SelectedDevicesListToPrintQrCodes.Any(d => d.DeviceID == selectedDevice.DeviceID))
            {
                SelectedDevicesListToPrintQrCodes.Add(selectedDevice);
            }
        }

        /// <summary>
        /// Method to get list of selected devices to print QR codes and clear the list after getting it. This method will be used in the page to print QR codes for selected devices. After getting the list of selected devices to print QR codes, the list will be cleared to avoid printing QR codes for the same devices again.
        /// </summary>
        /// <returns></returns>
        public List<DeviceProfile> GetCurrentQueue()
        {
            List<DeviceProfile> temporaryDevicesList = SelectedDevicesListToPrintQrCodes.ToList(); // Copy the list

            return temporaryDevicesList;                                // Return the copy
        }

        public void CleanCurrentQueue()
        {
            SelectedDevicesListToPrintQrCodes.Clear();
        }

        public void RemoveFromQueue(DeviceProfile device)
        {
            DeviceProfile? deviceToRemoveFromList = SelectedDevicesListToPrintQrCodes.FirstOrDefault(d => d.DeviceID == device.DeviceID);
            if (deviceToRemoveFromList != null)
            {
                SelectedDevicesListToPrintQrCodes.Remove(deviceToRemoveFromList);
            }
        }

        public async Task AddAllToPrintQueue()
        {
            List<DeviceProfile> allDevices = await GetAllUserDevicesAsync();
            foreach (var device in allDevices)
            {
                if (!SelectedDevicesListToPrintQrCodes.Any(d => d.DeviceID == device.DeviceID))
                {
                    SelectedDevicesListToPrintQrCodes.Add(device);
                }
            }
        }
        /// <summary>
        /// Getting item in the list by ID
        /// </summary>
        /// <param name="id">ID number</param>
        /// <returns>Device Profile from the list with matched device ID</returns>
    }
}
