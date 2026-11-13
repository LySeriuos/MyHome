using MyHome.Models;

namespace MyHome;

public static class QrCodeGenerator
{
        private static byte[] CreateQrCodeBytes(string payload)
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();

            using var qrCodeData = qrGenerator.CreateQrCode(
                payload,
                QRCoder.QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new QRCoder.PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }

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
}
