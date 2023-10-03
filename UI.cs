using MyHome;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using System.Runtime.Loader;

namespace My_Home
{
    internal class UI
    {

        public static DeviceProfile AddNewDevice(RealEstate userChosedRealEstateToMoveDevice)
        {
            TimeSpan interval;
            DeviceProfile device = new DeviceProfile();

            //Console.WriteLine("Add new Device Name");
            //device.DeviceName = Console.ReadLine();
            //Console.WriteLine("Add Model number");
            //device.DeviceModelNumber = Console.ReadLine();
            //Console.WriteLine("Add Serial number");
            //device.DeviceSerialNumber = Console.ReadLine();
            //Console.WriteLine("Add IP address");
            //device.IpAddress = Console.ReadLine();
            //Console.WriteLine("Add MAC address");
            //device.MacAdrress = Console.ReadLine();
            //Console.WriteLine("Add Device Produser");
            //device.DeviceProduser = Console.ReadLine();
            //Console.WriteLine("Add Link To Manual");
            //device.ManualBookLink = Console.ReadLine();

            DeviceWarranty deviceWarranty = device.DeviceWarranty;
            Console.WriteLine("Add warranty length");
            string warrantyPeriod = Console.ReadLine();
            TimeSpan time = new TimeSpan(0,0,0,0);
            //deviceWarranty.WarrantyPeriod = TimeSpan.ParseExact(warrantyPeriod, "%d");
            time = new TimeSpan(0,0,0,0);
            string gugu = time.ToString("c");
            TimeSpan giid = TimeSpan.Parse(gugu);
            deviceWarranty.WarrantyPeriod = giid;
            Console.WriteLine("{0} --> {1}", warrantyPeriod, giid.ToString("c"));

            //deviceWarranty.WarrantyPeriod = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Add link to receipt");
            deviceWarranty.ReceiptLink = Console.ReadLine();
            Console.WriteLine("Add extra insurance or warranty length");
            deviceWarranty.ExtraInsuranceWarrantyLenght = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Add purchase date");
            deviceWarranty.PurchaseDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Add extra insurance or warranty receipt link");
            deviceWarranty.ExtraInsuranceWarrantyLink = Console.ReadLine();

            Shop shop = deviceWarranty.Shop;            
            Console.WriteLine("Add Shop name");
            shop.ShopName = Console.ReadLine();
            Console.WriteLine("Add Shop number");
            shop.PhoneNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Add web address to the shop");
            shop.ShopWebAddress = Console.ReadLine();

            Address address = shop.Address;
            Console.WriteLine("Add shop street name");
            address.StreetName = Console.ReadLine();            
            Console.WriteLine("Addd shop house number");
            address.HouseNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Add shop house extended number/letter");
            address.HouseNumberExtension = Console.ReadLine();
            Console.WriteLine("Add shop city name");
            address.City = Console.ReadLine();
            Console.WriteLine("Add shop street name");
            address.Country = Console.ReadLine();



            userChosedRealEstateToMoveDevice.DevicesProfiles.Add(device);
            
            //user.RealEstates = Console.ReadLine();
            return device;
        }
    }
}
