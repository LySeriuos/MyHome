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

            // DevicesProfile

            DevicesProfile device1 = new DevicesProfile();
            device1.DeviceName = "Gaming computer";
            device1.DeviceType = DeviceType.Computer;
            device1.DeviceModelNumber = "NPC-I7-4070-0423";
            device1.DeviceSerialNumber = "7394291106279";
            device1.IpAdress = "58.31.187.181";
            device1.MacAdrress = "2E-A2-C8-A8-20-99";
            device1.DeviceProduser = "Samsung";
            device1.ManualBookLink = "https://www.bhphotovideo.com/lit_files/116441.pdf";
             
            house1.DevicesProfile.Add(device1);

            // Device Warranty

            DeviceWarranty deviceWarranty = new DeviceWarranty();
            deviceWarranty.ReceiptLink = "https://discuss.poynt.net/uploads/default/original/2X/6/60c4199364474569561cba359d486e6c69ae8cba.jpeg";
            deviceWarranty.ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            int deviceWarrantyPeriod = 1;
            deviceWarranty.ExtraDeviceInsuranceLength = 3;
            DateTime purchaseDate1 = new DateTime(2023, 7, 15);
            DateTime validUntil = purchaseDate1.AddYears(deviceWarrantyPeriod+deviceWarranty.ExtraDeviceInsuranceLength);
            // it is typical warranty in the USA is 1 year 
            // it should be enum bycountries because later it automatically count warranty period or ? 

            deviceWarranty.PurchaseDate = purchaseDate1;
            DateTime now = DateTime.Now;
            deviceWarranty.WarrantyPeriod = validUntil - now;
            device1.DeviceWarranty = deviceWarranty;

            Shop shop = new Shop();
            shop.ShopName = "Tesco";
            shop.PhoneNumber = 00441992632222;
            shop.ShopWebAdrress = "https://www.tescoplc.com/";
            deviceWarranty.Shop = shop;

            Address shopAddress = new Address();
            shopAddress.StreetName = "Indiana Ave";
            shopAddress.HouseNumber = 1925;
            shopAddress.City = "Salt Lake City";
            shopAddress.Country = "USA";
            deviceWarranty.Shop.Adrress = shopAddress;

            usersList.Add(userProfile1);


            return usersList;
        }
        
    }
}
