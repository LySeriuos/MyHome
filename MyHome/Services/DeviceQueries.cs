using MyHome.Models;

namespace MyHome;

public static class DeviceQueries
{
        public static List<DeviceProfile> GetAllUserDevices(UserProfile user)
        {
            List<DeviceProfile> allDevices = new List<DeviceProfile>();
            List<RealEstate> realEstates = user.RealEstates;
            var devices = realEstates.SelectMany(realEstate => realEstate.DevicesProfiles);
            foreach (DeviceProfile device in devices)
            {
                allDevices.Add(device);
            }
            return allDevices;
        }

        public static DeviceProfile GetDeviceInfoByID(int searchID, UserProfile currentUser)
        {
            var foundDevice = currentUser.GetAllDevices().Where(d => d.DeviceID == searchID).FirstOrDefault();
            return foundDevice;
        }
}
