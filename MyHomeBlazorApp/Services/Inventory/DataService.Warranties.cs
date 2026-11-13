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
        /// Method to get a device with closest expirig date to the actual date 
        /// </summary>
        /// <returns>expiringDevice device profile</returns>
        public DeviceProfile? FirstExpiringDeviceWarranty()
        {
            return _currentUserWithAllData?
                .RealEstates
                .SelectMany(r => r.DevicesProfiles)
                .Where(d =>
                    d.DeviceWarranty != null &&
                    d.DeviceWarranty.WarrantyEnd >= DateTime.Today)
                .OrderBy(d => d.DeviceWarranty.WarrantyEnd)
                .FirstOrDefault();
        }

        public static TimeSpan GetTimeSpanFromYears(int years) // add days from editform 
        {
            int totalDaysInTheYear = 365;
            int yearsToDays = years * totalDaysInTheYear;
            TimeSpan interval = TimeSpan.FromDays(yearsToDays);
            string timeInterval = interval.ToString();
            int pIndex = timeInterval.IndexOf(':');
            pIndex = timeInterval.IndexOf('.', pIndex);
            if (pIndex < 0) timeInterval += "        ";

            Console.WriteLine("{0,21}{1,26}", yearsToDays, timeInterval);
            return interval;
        }
    }
}
