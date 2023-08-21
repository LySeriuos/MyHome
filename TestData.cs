using MyHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace My_Home
{
    public static class TestData
    {

        public static List<UserProfile> User()
        {
            List<UserProfile> usersList = new List<UserProfile>();
            DateTime now = DateTime.Now;

            // User Profile
            UserProfile userProfile1 = new UserProfile();

            userProfile1.UserName = "James";
            userProfile1.Email = "james@gmail.com";
            userProfile1.House = new List<RealEstate>();
            // missing pasword method
            
            
            //Real Estate, User can have more than one place
            
            RealEstate house1 = new RealEstate();
            
            //Place name
            house1.RealEstateName = "James House";
            house1.DevicesProfile = new List<DevicesProfile>();
           
            //Address
            Address address1 = new Address();
            address1.StreetName = "Walton Street";
            address1.HouseNumber = 583;
            address1.City = "Salt Lake City";
            address1.Country = "Usa";            
            house1.Address = address1;
            userProfile1.House.Add(house1);
            // maybe I should put State, it could be useful

            //for test purposes
            foreach (RealEstate userProfile in userProfile1.House)
            {
                Console.WriteLine(userProfile.RealEstateName);
                Console.WriteLine(userProfile.Address.City);
            }

            // DevicesProfile 1

            DevicesProfile device1 = new DevicesProfile();
            device1.DeviceName = "Gaming Computer";
            device1.DeviceType = DeviceType.Computer;
            device1.DeviceModelNumber = "NPC-I7-4070-0423";
            device1.DeviceSerialNumber = "7394291106279";
            device1.IpAdress = "58.31.187.181";
            device1.MacAdrress = "2E-A2-C8-A8-20-99";
            device1.DeviceProduser = "Samsung";
            device1.ManualBookLink = "https://www.bhphotovideo.com/lit_files/116441.pdf";             
            house1.DevicesProfile.Add(device1);

            
            // Device Warranty 1

            DeviceWarranty deviceWarranty1 = new DeviceWarranty();
            deviceWarranty1.ReceiptLink = "https://discuss.poynt.net/uploads/default/original/2X/6/60c4199364474569561cba359d486e6c69ae8cba.jpeg";
            deviceWarranty1.ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            int deviceWarrantyPeriod1 = 1;
            deviceWarranty1.ExtraDeviceInsuranceLength = 3;
            DateTime purchaseDate1 = new DateTime(2023, 7, 15);
            DateTime validUntil1 = purchaseDate1.AddYears(deviceWarrantyPeriod1+deviceWarranty1.ExtraDeviceInsuranceLength);
            // it is typical warranty in the USA is 1 year 
            // it should be enum bycountries because later it automatically count warranty period or ? 

            // counting the length of left time for warranty
            deviceWarranty1.PurchaseDate = purchaseDate1;
            now = DateTime.Now;
            deviceWarranty1.WarrantyPeriod = validUntil1 - now;
            device1.DeviceWarranty = deviceWarranty1;

            // Shop 1

            Shop shop = new Shop();
            shop.ShopName = "Tesco";
            shop.PhoneNumber = 00441992632222;
            shop.ShopWebAdrress = "https://www.tescoplc.com/";
            deviceWarranty1.Shop = shop;

            // Shop Address 1

            Address shopAddress1 = new Address();
            shopAddress1.StreetName = "Indiana Ave";
            shopAddress1.HouseNumber = 1925;
            shopAddress1.City = "Salt Lake City";
            shopAddress1.Country = "USA";
            deviceWarranty1.Shop.Adrress = shopAddress1;

            // DevicesProfile 2

            DevicesProfile device2 = new DevicesProfile();
            device2.DeviceName = "James Phone";
            device2.DeviceType = DeviceType.MobileDevice;
            device2.DeviceModelNumber = "A2891";
            device2.DeviceSerialNumber = "HM95O92P2F";
            device2.IpAdress = "192.168. 1.13";
            device2.MacAdrress = "00-B0-D0-63-C2-26";
            device2.DeviceProduser = "Apple";
            device2.ManualBookLink = "https://support.apple.com/sv-se/guide/iphone/iphfc2d9bc6a/ios";
            house1.DevicesProfile.Add(device2);

            // Device Warranty 2 

            DeviceWarranty deviceWarranty2 = new DeviceWarranty();
            deviceWarranty2.ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf";
            deviceWarranty2.ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            int deviceWarrantyPeriod = 1;
            deviceWarranty2.ExtraDeviceInsuranceLength = 1;
            DateTime purchaseDate2 = new DateTime(2023, 1, 23);
            DateTime validUntil2 = purchaseDate1.AddYears(deviceWarrantyPeriod + deviceWarranty2.ExtraDeviceInsuranceLength);
            deviceWarranty2.PurchaseDate = purchaseDate2;
            now = DateTime.Now;
            deviceWarranty2.WarrantyPeriod = validUntil1 - now;
            device2.DeviceWarranty = deviceWarranty1;

            // Shop 2

            Shop shop2 = new Shop();
            shop2.ShopName = "T-Mobile";
            shop2.PhoneNumber = 18009378997;
            shop2.ShopWebAdrress = "https://www.t-mobile.com/?INTNAV=tNav:Home";
            deviceWarranty2.Shop = shop;

            // Shop Address

            Address shopAddress2 = new Address();
            shopAddress2.StreetName = "Indiana Ave";
            shopAddress2.HouseNumber = 1925;
            shopAddress2.City = "Salt Lake City";
            shopAddress2.Country = "USA";
            deviceWarranty2.Shop.Adrress = shopAddress;


            usersList.Add(userProfile1);


            return usersList;
        }
        
    }
}
