using MyHome.Models;
using System.IO;
using SkiaSharp;
using SkiaSharp.QrCode;

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
                    string userDevice = device.DeviceName;
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
                        expiringDevices = expiringDevices.OrderBy(d => d.DeviceWarranty.WarrantyEnd).ToList();
                    }
                }
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
        /// <summary>
        /// New method to create Qr code as byte array to send it to the client side and generate image there without saving it on the server side, because of security reasons and to avoid problems with file management on the server side. This method will be used in the PdfController to generate Qr code for each device in the PDF file.
        /// </summary>
        /// <param name="deviceID">chosed device ID to generate Qr code for</param>
        /// <param name="userID">current user ID</param>
        /// <returns>byte array representing the Qr code image</returns>
        public static byte[] GetQrCodeBytes(int deviceID, int userID)
        {
            // Encode to Base64
            string secretDeviceID = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deviceID.ToString()));
            string secretUserID = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userID.ToString()));

            // Create URL
            var qrCodePayload = new QRCoder.PayloadGenerator.Url($"https://85.215.169.44/mobileDeviceInfo/{secretUserID}/{secretDeviceID}");

            // Generate Graphic as Bytes
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrCodePayload.ToString(), QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20); // Returns byte[]
        }

        /// <summary>
        /// Method to create QrCode Link to reach devices info  
        /// </summary>
        /// <param name="deviceID">Qr Code link to the device by device ID </param>
        /// <param name="userID">current user by user ID </param>
        /// <param name="savedQrCodeLink">Path to saved Qr Code</param>
        public static void CreateQrCodeLinkToDevice(string deviceID, string userID, string savedQrCodeLink)
        {
            string secretDeviceID = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deviceID.ToString()));
            string secretUserID = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userID.ToString()));
            //var url = $"http://your-ip/mobileDeviceInfo/{secretUserID}/{secretDeviceID}";
            // TODO: change Ip Address to the real after it will be conneected to the domain name
            var qrCodePayload = new QRCoder.PayloadGenerator.Url($"https://85.215.169.44/mobileDeviceInfo/{secretUserID}/{secretDeviceID}");

            string generatedQrCodeLink = qrCodePayload.ToString();

            // Call your static generator method
            QrCodeGenerator(generatedQrCodeLink, savedQrCodeLink);
        }

        /// <summary>
        /// Create and save Qr code image
        /// </summary>
        /// <param name="generatedQrCodeLink">Generated qr code link to the url address </param>
        /// <param name="savedQrCodeLink">link of saved Qr code on local mashine</param>
        //public static void QrCodeGenerator(string generatedQrCodeLink, string savedQrCodeLink)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(generatedQrCodeLink, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    Bitmap qrCodeImage = qrCode.GetGraphic(20);
        //    qrCodeImage.Save(savedQrCodeLink);
        //}



        // Inside your Logic class

        public static void QrCodeGenerator(string generatedQrCodeLink, string savedQrCodeLink)
        {
            // Use the full namespace to avoid the conflict with QRCoder
            var qrCodeData = SkiaSharp.QrCode.QRCodeGenerator.CreateQrCode(generatedQrCodeLink, SkiaSharp.QrCode.ECCLevel.Q);

            var info = new SKImageInfo(512, 512);
            using var surface = SKSurface.Create(info);
            using var canvas = surface.Canvas;

            var rect = new SKRect(0, 0, info.Width, info.Height);

            // Use the full namespace here as well
            SkiaSharp.QrCode.QRCodeRenderer.Render(canvas, rect, qrCodeData, SKColors.White, SKColors.Black);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var folder = Path.GetDirectoryName(savedQrCodeLink);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            using var stream = File.Open(savedQrCodeLink, FileMode.Create, FileAccess.Write);
            data.SaveTo(stream);
        }



        public static void CreateWifiSharingQrCode(string wifiName, string wifiPassword, string savedQrCodeLink)
        {
            // Explicitly define the WiFi payload type from QRCoder
            QRCoder.PayloadGenerator.WiFi wifiPayLoad = new QRCoder.PayloadGenerator.WiFi(
                wifiName,
                wifiPassword,
                QRCoder.PayloadGenerator.WiFi.Authentication.WPA
            );

            string generatedWifiQrCodeLink = wifiPayLoad.ToString();

            // This calls your SkiaSharp generator method we fixed earlier
            QrCodeGenerator(generatedWifiQrCodeLink, savedQrCodeLink);
        }

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
