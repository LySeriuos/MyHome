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
            DateTime now;

            // User Profile 1
            #region User1 
            UserProfile userProfile1 = new UserProfile();

            userProfile1.UserName = "James";
            userProfile1.Email = "james@gmail.com";
            userProfile1.House = new List<RealEstate>();
            // missing pasword method
            
            
            //Real Estate 1, User can have more than one place
            
            RealEstate jamesHouse1 = new RealEstate();
            
            //Place name 1

            jamesHouse1.RealEstateName = "James House";
            jamesHouse1.DevicesProfile = new List<DevicesProfile>();
           
            //Address RealEstate 1

            Address jamesHouse1Address = new Address();
            jamesHouse1Address.StreetName = "Walton Street";
            jamesHouse1Address.HouseNumber = 583;
            jamesHouse1Address.City = "Salt Lake City";
            jamesHouse1Address.Country = "Usa";            
            jamesHouse1.Address = jamesHouse1Address;

     
            var houseTest = new Address()
            {
                ApartamentNumber = 5,
                City = "Vienna",
                Country = "austria"
            };
            
            // maybe I should put State, it could be useful

            //for test purposes
            foreach (RealEstate userProfile in userProfile1.House)
            {
                Console.WriteLine(userProfile.RealEstateName);
                Console.WriteLine(userProfile.Address.City);
            }

            // DevicesProfile 1

            DevicesProfile jamesDevice1 = new DevicesProfile();
            jamesDevice1.DeviceName = "Gaming Computer";
            jamesDevice1.DeviceType = DeviceType.Computer;
            jamesDevice1.DeviceModelNumber = "NPC-I7-4070-0423";
            jamesDevice1.DeviceSerialNumber = "7394291106279";
            jamesDevice1.IpAddress = "58.31.187.181";
            jamesDevice1.MacAdrress = "2E-A2-C8-A8-20-99";
            jamesDevice1.DeviceProduser = "Samsung";
            jamesDevice1.ManualBookLink = "https://www.bhphotovideo.com/lit_files/116441.pdf";             
            

            // Device Warranty 1

            DeviceWarranty jamesDevice1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://discuss.poynt.net/uploads/default/original/2X/6/60c4199364474569561cba359d486e6c69ae8cba.jpeg",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };

            jamesDevice1Warranty.WarrantyPeriod = new TimeSpan(365, 0, 0, 0, 0);
            jamesDevice1Warranty.ExtendedWarranty = new TimeSpan(1095, 0, 0, 0);
            DateTime purchaseDate1 = new DateTime(2023, 7, 15);
            
            // it is typical warranty in the USA is 1 year 
            // it should be enum bycountries because later it automatically count warranty period or ? 

            // counting the length of left time for warranty
            jamesDevice1Warranty.PurchaseDate = purchaseDate1;
            now = DateTime.Now;
            
            jamesDevice1.DeviceWarranty = jamesDevice1Warranty;

            // Shop 1

            Shop shop1 = new Shop();
            shop1.ShopName = "Tesco";
            shop1.PhoneNumber = 00441992632222;
            shop1.ShopWebAdrress = "https://www.tescoplc.com/";
            jamesDevice1Warranty.Shop = shop1;

            // Shop Address 1

            Address shopAddress1 = new Address();
            shopAddress1.StreetName = "Indiana Ave";
            shopAddress1.HouseNumber = 1925;
            shopAddress1.City = "Salt Lake City";
            shopAddress1.Country = "USA";
            jamesDevice1Warranty.Shop.Adrress = shopAddress1;

            // Adding Device object to the RealEstate list

            jamesHouse1.DevicesProfile.Add(jamesDevice1);

            // DevicesProfile 2

            DevicesProfile jamesDevice2 = new DevicesProfile();
            jamesDevice2.DeviceName = "James Phone";
            jamesDevice2.DeviceType = DeviceType.MobileDevice;
            jamesDevice2.DeviceModelNumber = "A2891";
            jamesDevice2.DeviceSerialNumber = "HM95O92P2F";
            jamesDevice2.IpAddress = "192.168.1.13";
            jamesDevice2.MacAdrress = "00-B0-D0-63-C2-26";
            jamesDevice2.DeviceProduser = "Apple";
            jamesDevice2.ManualBookLink = "https://support.apple.com/sv-se/guide/iphone/iphfc2d9bc6a/ios";
            

            // Device Warranty 2 

            DeviceWarranty jamesDevice2Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            jamesDevice2Warranty.WarrantyPeriod = new TimeSpan(365, 0, 0, 0, 0);
            jamesDevice2Warranty.ExtendedWarranty = new TimeSpan(365, 0, 0, 0, 0);
            DateTime purchaseDate2 = new DateTime(2023, 1, 23);            
            jamesDevice2Warranty.PurchaseDate = purchaseDate2;
            now = DateTime.Now;
            
            jamesDevice2.DeviceWarranty = jamesDevice2Warranty;

            // Shop 2

            Shop shop2 = new Shop();
            shop2.ShopName = "T-Mobile";
            shop2.PhoneNumber = 18009378997;
            shop2.ShopWebAdrress = "https://www.t-mobile.com/?INTNAV=tNav:Home";
            jamesDevice2Warranty.Shop = shop2;

            // Shop 2 Address

            Address shop2Address = new Address();
            shop2Address.StreetName = "1007 N Main St";
            shop2Address.HouseNumber = 110;
            shop2Address.City = "Logan";
            shop2Address.Country = "USA";
            jamesDevice2Warranty.Shop.Adrress = shop2Address;

            // Adding Device object to the RealEstate list

            jamesHouse1.DevicesProfile.Add(jamesDevice2);

            // Adding RealEstate object to the userProfile List

            userProfile1.House.Add(jamesHouse1);

            // Adding userProfile to the user List
            usersList.Add(userProfile1);


#endregion
            ////////// User Profile 2 ///////////////////////
            #region User2

            UserProfile userProfile2 = new UserProfile();

            userProfile2.UserName = "John";
            userProfile2.Email = "john@gmail.com";
            userProfile2.House = new List<RealEstate>();
            // missing pasword method

            RealEstate johnHouse1 = new RealEstate();

            //Place name 1

            johnHouse1.RealEstateName = "John Apartament";
            johnHouse1.DevicesProfile = new List<DevicesProfile>();

            //Address Realestate 1

            Address johnHouse1Address = new Address();
            johnHouse1Address.StreetName = "Valldammsgatan";
            johnHouse1Address.HouseNumber = 96;
            johnHouse1Address.City = "Gusum";
            johnHouse1Address.Country = "Sweden";
            johnHouse1.Address = johnHouse1Address;
            
            // maybe I should put State, it could be useful

            // DevicesProfile 1

            DevicesProfile johnHouse1Device1 = new DevicesProfile();
            johnHouse1Device1.DeviceName = "DishWasher";
            johnHouse1Device1.DeviceType = DeviceType.Kitchen;
            johnHouse1Device1.DeviceModelNumber = "DBI8557MIMXXLBS";
            johnHouse1Device1.DeviceSerialNumber = "131002350324";
            johnHouse1Device1.IpAddress = "228.215.193.124";
            johnHouse1Device1.MacAdrress = "87-86-1F-E0-A3-A6";
            johnHouse1Device1.DeviceProduser = "Asko";
            johnHouse1Device1.ManualBookLink = "https://www.bruksanvisni.ng/asko/dbi8557mimxxls/bruksanvisning";
            

            // Device Warranty 1 

            DeviceWarranty johnHouse1Device1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://img.freepik.com/free-vector/receipt-template-collection-with-realistic-design_23-2147910552.jpg?w=1380&t=st=1692650469~exp=1692651069~hmac=91e29da374bc7eddec6ec6c10e9cb828b93ac76cf4c79d558a6ea1257ef728a4",
                ExtraDeviceInsuranceLink = "https://img.yumpu.com/20322498/1/500x640/ladda-ned-produktblad-som-pdf.jpg"
            };
            johnHouse1Device1Warranty.WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0);
            johnHouse1Device1Warranty.ExtendedWarranty = new TimeSpan(1095, 0, 0, 0, 0);
            DateTime purchaseDate3 = new DateTime(2022, 6, 10);
            
            johnHouse1Device1Warranty.PurchaseDate = purchaseDate3;
            now = DateTime.Now;
            
            johnHouse1Device1.DeviceWarranty = johnHouse1Device1Warranty;

            // Shop 1

            Shop shop3 = new Shop();
            shop3.ShopName = "Elgiganten";
            shop3.PhoneNumber = 0771115115;
            shop3.ShopWebAdrress = "https://www.elgiganten.se/";
            johnHouse1Device1Warranty.Shop = shop3;

            // Shop 1 Address

            Address shop3Address = new Address();
            shop3Address.StreetName = "Norra Svedengatan";
            shop3Address.HouseNumber = 19;
            shop3Address.City = "Linköping";
            shop3Address.Country = "Sweden";
            johnHouse1Device1Warranty.Shop.Adrress = shop3Address;
            
            // Adding Device object to the RealEstate list
            johnHouse1.DevicesProfile.Add(johnHouse1Device1);

            // Adding RealEstate object to the userProfile List
            userProfile2.House.Add(johnHouse1);


            RealEstate johnHouse2 = new RealEstate();

            //Place name 2

            johnHouse2.RealEstateName = "John House Rental";
            johnHouse2.DevicesProfile = new List<DevicesProfile>();

            //Address Realestate 2

            Address johnHouse2Address = new Address();
            johnHouse2Address.StreetName = "Vipgränden";
            johnHouse2Address.HouseNumber = 81;
            johnHouse2Address.HouseNumberExtension = "A";
            johnHouse2Address.City = "Gullringen";
            johnHouse2Address.Country = "Sweden";
            johnHouse2.Address = johnHouse2Address;
            // maybe I should put State, it could be useful

            // Realestate 2 DevicesProfile 1

            DevicesProfile johnHouse2Device1 = new DevicesProfile();
            johnHouse2Device1.DeviceName = "Washing-Drying Machine";
            johnHouse2Device1.DeviceType = DeviceType.Cleaning; // could be gruoped as multidevice but still it should be grouped as Cleaning device
            johnHouse2Device1.DeviceModelNumber = "CV90V6S2BA";
            johnHouse2Device1.DeviceSerialNumber = "1ABC2DEF345678G";
            johnHouse2Device1.IpAddress = "188.188.72.36";
            johnHouse2Device1.MacAdrress = "F8-15-50-CC-A3-6E";
            johnHouse2Device1.DeviceProduser = "LG";
            // local file
            johnHouse2Device1.ManualBookLink = "file:///C:/Users/shiranco.DESKTOP-HRN41TE/Downloads/[EPREL_20230519175627742_20230519175627742]%20(1).pdf";


            // Realestate 2 Device1 Warranty 

            DeviceWarranty johnHouse2Device1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            johnHouse2Device1Warranty.WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0);
            johnHouse2Device1Warranty.ExtendedWarranty = new TimeSpan(365, 0, 0, 0, 0);
            DateTime purchaseDate4 = new DateTime(2021, 1, 14);
            
            johnHouse2Device1Warranty.PurchaseDate = purchaseDate4;
            now = DateTime.Now;
            
            johnHouse2Device1.DeviceWarranty = johnHouse2Device1Warranty;

            // Realestate 2 Shop 1

            Shop shop4 = new Shop();
            shop4.ShopName = "Elgiganten";
            shop4.PhoneNumber = 0771115115;
            shop4.ShopWebAdrress = "https://www.elgiganten.se/";
            johnHouse1Device1Warranty.Shop = shop4;

            // Realestate 2 Shop 1 Address

            Address shop4Address = new Address();
            shop4Address.StreetName = "Ljunghedsvägen";
            shop4Address.HouseNumber = 7;
            shop4Address.City = "Västervik";
            shop4Address.Country = "Sweden";
            johnHouse1Device1Warranty.Shop.Adrress = shop4Address;

            // Adding Device object to the RealEstate list
            johnHouse2.DevicesProfile.Add(johnHouse2Device1);

            // Realestate 2 DevicesProfile 2

            DevicesProfile johnHouse2Device2 = new DevicesProfile();
            johnHouse2Device2.DeviceName = "Main Bedroom TV";
            johnHouse2Device2.DeviceType = DeviceType.Video;
            johnHouse2Device2.DeviceModelNumber = "TQ55LS03BGUXXC";
            johnHouse2Device2.DeviceSerialNumber = "AKZ43CPQ505923D";
            johnHouse2Device2.IpAddress = "134.121.255.219";
            johnHouse2Device2.MacAdrress = "B5-76-8F-F6-7A-1B";
            johnHouse2Device2.DeviceProduser = "Samsung";
            johnHouse2Device2.ManualBookLink = "https://www.samsung.com/uk/support/model/QE55LS03BGUXXU/#downloads";


            // Realestate 2 Device 2 Warranty  

            DeviceWarranty johnHouse2Device2Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://qph.cf2.quoracdn.net/main-qimg-8aa597ee2d773fce151545846c9d08a0-pjlq",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            johnHouse2Device2Warranty.WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0);
            johnHouse2Device2Warranty.ExtendedWarranty = new TimeSpan(365, 0, 0, 0, 0);
            DateTime purchaseDate5 = new DateTime(2021, 3, 23);
            
            johnHouse2Device2Warranty.PurchaseDate = purchaseDate5;
            now = DateTime.Now;
            
            johnHouse2Device2.DeviceWarranty = johnHouse2Device2Warranty;

            // Realestate 2 Shop 2

            Shop shop5 = new Shop();
            shop5.ShopName = "Elgiganten";
            shop5.PhoneNumber = 0771115115;
            shop5.ShopWebAdrress = "https://www.elgiganten.se/";
            johnHouse2Device2Warranty.Shop = shop5;

            // Real Estate 2 Shop 2 Address

            // if it is the same shop?
            Address shop5Address = new Address();
            shop5Address.StreetName = "Ljunghedsvägen";
            shop5Address.HouseNumber = 7;
            shop5Address.City = "Västervik";
            shop5Address.Country = "Sweden";
            johnHouse2Device2Warranty.Shop.Adrress = shop5Address;

            // Adding Device object to the RealEstate list
            johnHouse2.DevicesProfile.Add(johnHouse2Device2);

            // Adding RealEstate object to the userProfile List
            userProfile2.House.Add(johnHouse2);
            #endregion
            // Adding userProfile to the user List
            usersList.Add(userProfile2);

            return usersList;
        }
        
    }
}
