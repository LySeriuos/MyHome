using MyHome.Models;

namespace MyHome;

public static class LegacyLogic
{
        public static bool MoveDeviceToOtherRealEstate(UserProfile user, int deviceID, RealEstate userChosedRealEstateToMoveDevice)
        {
            RealEstate targetRealEstate = userChosedRealEstateToMoveDevice;
            List<RealEstate> realEstateList = user.RealEstates;
            List<DeviceProfile> devices = new List<DeviceProfile>();
            DeviceProfile foundDevice = new();
            bool deviceWasAdded = false;

            foreach (RealEstate sourceRealestate in realEstateList)
            {
                devices = sourceRealestate.DevicesProfiles;
                foundDevice = devices.Where(d => d.DeviceID == deviceID).FirstOrDefault();
                break;
            }

            if (!targetRealEstate.DevicesProfiles.Contains(foundDevice))
            {
                userChosedRealEstateToMoveDevice.DevicesProfiles.Add(foundDevice);
                deviceWasAdded = true;
            }
            else
            {
                deviceWasAdded = false;
            }
            devices.Remove(foundDevice);
            return deviceWasAdded;
        }

        public static void RemoveDevice(RealEstate currentRealEstate, DeviceProfile deviceToDelete, List<UserProfile> _users, string _path)
        {
            currentRealEstate.DevicesProfiles.Remove(deviceToDelete);
            XMLFileUitilities.SaveUsersListToXml(_users, _path);
        }

        public static int GetUserMaxId(List<UserProfile> userProfiles)
        {
            int maxID = userProfiles.Max(u => u.UserID);
            return maxID;
        }

        public static int GetRealEstateMaxId(List<RealEstate> realEstates)
        {
            int maxID;
            if (realEstates.Count == 0)
            {
                maxID = 0;
            }
            else
            {
                maxID = realEstates.Max(rE => rE.RealEstateID);
            }
            return maxID;
        }

        public static int GetDeviceMaxId(List<DeviceProfile> devices)
        {
            int maxID;
            if (devices == null)
            {
                maxID = 0;
            }
            else
            {
                maxID = devices.Max(d => d.DeviceID);
            }
            return maxID;
        }
}
