using MyHome.Models;

namespace MyHome
{
    public class Logic
    {
        public const int DAYS_TILL_WARRANTY_ENDS = 180;


        /// <summary>
        /// Method to get all devices with warranties which ending soon
        /// </summary>
        /// <param name="userProfile">Devices connected to Users</param>
        /// <param name="daysTillEnd">Max days until devices warranties ends</param>
        /// <returns>List of devices which are in the period of Max days to warraties ends</returns>
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

        /// <summary>
        /// Geting all user's devices
        /// </summary>
        /// <param name="user"> Users </param>
        /// <returns>List of user devices</returns>
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

        /// <summary>
        /// Geting user's devices expiring warranties dates
        /// </summary>
        /// <param name="user">Users</param>
        /// <returns>List of devices warranties</returns>
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

        /// <summary>
        /// Method to move device object from one Real Estate to another Real Estatee by device's serial number
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="userChosedRealEstateToMoveDevice">Chosed Real Estate where to move the device</param>
        /// <returns>bool to give a respons for user if device moving was successful </returns>
        /// 
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
        
        // QR Code Section Start

        /// <summary>
        /// Generates QrCode image
        /// </summary>
        /// <param name="payload">data to add into qrcode</param>
        /// <returns>QrCode image</returns>
        private static byte[] CreateQrCodeBytes(string payload)
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();

            using var qrCodeData = qrGenerator.CreateQrCode(
                payload,
                QRCoder.QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new QRCoder.PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }

        /// <summary>
        /// Creates link to the qrcode
        /// </summary>
        /// <param name="userID">Current user</param>
        /// <param name="deviceID">Current device</param>
        /// <param name="baseUrl">base url to open created qr code</param>
        /// <returns></returns>
        public static byte[] GetQrCodeBytes(
    int userID,
    int deviceID,
    string baseUrl)
        {
            string url =
                $"{baseUrl.TrimEnd('/')}/mobileDeviceInfo/{userID}/{deviceID}";

            var urlPayload = new QRCoder.PayloadGenerator.Url(url);

            return CreateQrCodeBytes(urlPayload.ToString());
        }

        /// <summary>
        /// Creates link to the wifi qr code
        /// </summary>
        /// <param name="wifiName">Added Wifi name</param>
        /// <param name="wifiPassword">Added Password</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">error message </exception>
        public static string CreateWifiSharingQrCode(
    string wifiName,
    string wifiPassword)
        {
            if (string.IsNullOrWhiteSpace(wifiName))
            {
                throw new ArgumentException(
                    "Wi-Fi name is required.",
                    nameof(wifiName));
            }

            var wifiPayload = new QRCoder.PayloadGenerator.WiFi(
                wifiName.Trim(),
                wifiPassword ?? string.Empty,
                QRCoder.PayloadGenerator.WiFi.Authentication.WPA);

            byte[] imageBytes =
                CreateQrCodeBytes(wifiPayload.ToString());

            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }

        // Qr Code Section end

        /// <summary>
        /// Method to remove device from the xml database
        /// </summary>
        /// <param name="currentRealEstate">real estate where device is</param>
        /// <param name="deviceToDelete">chosed device to delete</param>
        /// <param name="_users">list of all users</param>
        /// <param name="_path">location destination where list will be saved</param>
        public static void RemoveDevice(RealEstate currentRealEstate, DeviceProfile deviceToDelete, List<UserProfile> _users, string _path)
        {
            currentRealEstate.DevicesProfiles.Remove(deviceToDelete);
            XMLFileUitilities.SaveUsersListToXml(_users, _path);
        }

        /// <summary>
        /// Method to get DeviceProfile by ID
        /// </summary>
        /// <param name="searchID">get device with this ID</param>
        /// <param name="currentUser">user profile</param>
        /// <returns>device profile</returns>
        public static DeviceProfile GetDeviceInfoByID(int searchID, UserProfile currentUser)
        {
            var foundDevice = currentUser.GetAllDevices().Where(d => d.DeviceID == searchID).FirstOrDefault();
            return foundDevice;
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
}
