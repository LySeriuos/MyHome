using MyHome.Models;

namespace MyHome;

public static class WarrantyQueries
{
    public const int DAYS_TILL_WARRANTY_ENDS = 180;

        public static List<DeviceProfile> ExpiringDevicesWarrantiesInDays(UserProfile userProfile, int DAYS_TILL_WARRANTY_ENDS)
        {
            List<DeviceProfile> expiringDevices = new List<DeviceProfile>();
            List<RealEstate> realEstates = userProfile.RealEstates;
            foreach (RealEstate realEstate in realEstates)
            {
                List<DeviceProfile> devices = realEstate.DevicesProfiles;
                foreach (DeviceProfile device in devices)
                {
                    DeviceWarranty warranty = device.DeviceWarranty;
                    if (warranty != null)
                    {
                        DateTime deviceWarranty = warranty.WarrantyEnd;
                        DateTime dateTime = DateTime.Now;
                        TimeSpan daysCounting = deviceWarranty.Subtract(dateTime);
                        if (daysCounting.Days < DAYS_TILL_WARRANTY_ENDS)
                        {
                            expiringDevices.Add(device);
                        }
                    }
                }
                expiringDevices = expiringDevices.OrderBy(d => d.DeviceWarranty.WarrantyEnd).ToList();
            }

            return expiringDevices;
        }

        public static List<DeviceWarranty> GetUserDevicesWarranties(UserProfile user)
        {
            List<DeviceWarranty> devicesWarranties = new List<DeviceWarranty>();
            List<RealEstate> realEstates = user.RealEstates;
            DeviceWarranty warranty = new DeviceWarranty();
            foreach (RealEstate realestate in realEstates)
            {
                List<DeviceProfile> devices = realestate.DevicesProfiles;
                foreach (DeviceProfile device in devices)
                {
                    warranty = device.DeviceWarranty;
                    devicesWarranties.Add(warranty);
                }
            }
            return devicesWarranties;
        }
}
