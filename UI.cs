using MyHome;
using System.Runtime.Loader;

namespace My_Home
{
    internal class UI
    {

        public static DeviceProfile AddNewDevice()
        {
            DeviceProfile device = new();            
            Console.WriteLine("Add new Device Name");
            device.DeviceName = Console.ReadLine();
            Console.WriteLine("Add Device Type. 0 Audio, 1 Computer, 2 Kitchen, 3 Mobile Device, 4 Bathroom, 5 Cleaning, 6 Video, 7 Garden, 8 Security, 9 Multi Device, Other as a Default");
            int chosedDeviceType = Int32.Parse(Console.ReadLine());
            device.DeviceType = GetEnumOfDeviceType.SelectDeviceType(chosedDeviceType);           
            Console.WriteLine("Add Model number");
            device.DeviceModelNumber = Console.ReadLine();
            Console.WriteLine("Add Serial number");
            device.DeviceSerialNumber = Console.ReadLine();
            Console.WriteLine("Add IP address");
            device.IpAddress = Console.ReadLine();
            Console.WriteLine("Add MAC address");
            device.MacAdrress = Console.ReadLine();
            Console.WriteLine("Add Device Produser");
            device.DeviceProduser = Console.ReadLine();
            Console.WriteLine("Add Link To Manual");
            device.ManualBookLink = Console.ReadLine();

            DeviceWarranty deviceWarranty = new();

            Console.WriteLine("Add warranty length");
            deviceWarranty.WarrantyPeriod = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Add link to receipt");
            deviceWarranty.ReceiptLink = Console.ReadLine();
            Console.WriteLine("Add extra insurance or warranty length");
            deviceWarranty.ExtraInsuranceWarrantyLenght = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Add purchase date");
            deviceWarranty.PurchaseDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Add extra insurance or warranty receipt link");
            deviceWarranty.ExtraInsuranceWarrantyLink = Console.ReadLine();
            device.DeviceWarranty = deviceWarranty;

            Shop shop = new();          
            Console.WriteLine("Add Shop name");
            shop.ShopName = Console.ReadLine();
            Console.WriteLine("Add Shop number");
            shop.PhoneNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Add web address to the shop");
            shop.ShopWebAddress = Console.ReadLine();
            deviceWarranty.Shop = shop;

            Address address = new();
            Console.WriteLine("Add shop street name");
            address.StreetName = Console.ReadLine();            
            Console.WriteLine("Addd shop house number");
            address.HouseNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Add shop house extended number/letter");
            address.HouseNumberExtension = Console.ReadLine();
            Console.WriteLine("Add shop city name");
            address.City = Console.ReadLine();
            Console.WriteLine("Add Shop Country");
            address.Country = Console.ReadLine();
            shop.Address = address;

            return device;
        }
    }
}
