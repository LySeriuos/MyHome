﻿using MyHome;
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

            // User Profile 1
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
           
            //Address 1

            Address jamesHouse1Address = new Address();
            jamesHouse1Address.StreetName = "Walton Street";
            jamesHouse1Address.HouseNumber = 583;
            jamesHouse1Address.City = "Salt Lake City";
            jamesHouse1Address.Country = "Usa";            
            jamesHouse1.Address = jamesHouse1Address;
            
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
            jamesDevice1.IpAdress = "58.31.187.181";
            jamesDevice1.MacAdrress = "2E-A2-C8-A8-20-99";
            jamesDevice1.DeviceProduser = "Samsung";
            jamesDevice1.ManualBookLink = "https://www.bhphotovideo.com/lit_files/116441.pdf";             
            


            // Device Warranty 1

            DeviceWarranty jamesDevice1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://discuss.poynt.net/uploads/default/original/2X/6/60c4199364474569561cba359d486e6c69ae8cba.jpeg",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            int jamesDevice1WarrantyPeriod = 1;
            jamesDevice1Warranty.ExtraDeviceInsuranceLength = 3;
            DateTime purchaseDate1 = new DateTime(2023, 7, 15);
            DateTime validUntil1 = purchaseDate1.AddYears(jamesDevice1WarrantyPeriod+jamesDevice1Warranty.ExtraDeviceInsuranceLength);
            // it is typical warranty in the USA is 1 year 
            // it should be enum bycountries because later it automatically count warranty period or ? 

            // counting the length of left time for warranty
            jamesDevice1Warranty.PurchaseDate = purchaseDate1;
            now = DateTime.Now;
            jamesDevice1Warranty.WarrantyPeriod = validUntil1 - now;
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
            jamesDevice2.IpAdress = "192.168.1.13";
            jamesDevice2.MacAdrress = "00-B0-D0-63-C2-26";
            jamesDevice2.DeviceProduser = "Apple";
            jamesDevice2.ManualBookLink = "https://support.apple.com/sv-se/guide/iphone/iphfc2d9bc6a/ios";
            jamesHouse1.DevicesProfile.Add(jamesDevice2);

            // Device Warranty 2 

            DeviceWarranty jamesDevice2Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            int jamesDevice2WarrantyPeriod = 1;
            jamesDevice2Warranty.ExtraDeviceInsuranceLength = 1;
            DateTime purchaseDate2 = new DateTime(2023, 1, 23);
            DateTime validUntil2 = purchaseDate1.AddYears(jamesDevice2WarrantyPeriod + jamesDevice2Warranty.ExtraDeviceInsuranceLength);
            jamesDevice2Warranty.PurchaseDate = purchaseDate2;
            now = DateTime.Now;
            jamesDevice2Warranty.WarrantyPeriod = validUntil2 - now;
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



            ////////// User Profile 2 ///////////////////////


            UserProfile userProfile2 = new UserProfile();

            userProfile2.UserName = "John";
            userProfile2.Email = "john@gmail.com";
            userProfile2.House = new List<RealEstate>();
            // missing pasword method

            RealEstate johnHouse1 = new RealEstate();

            //Place name 1

            johnHouse1.RealEstateName = "John Apartament";
            johnHouse1.DevicesProfile = new List<DevicesProfile>();

            

            //Address 1

            Address johnHouse1Address = new Address();
            johnHouse1Address.StreetName = "Valldammsgatan";
            johnHouse1Address.HouseNumber = 96;
            johnHouse1Address.City = "Gusum";
            johnHouse1Address.Country = "Sweden";
            johnHouse1.Address = johnHouse1Address;
            
            // maybe I should put State, it could be useful

            // DevicesProfile 1

            DevicesProfile johnApartamentDevice1 = new DevicesProfile();
            johnApartamentDevice1.DeviceName = "DishWasher";
            johnApartamentDevice1.DeviceType = DeviceType.Kitchen;
            johnApartamentDevice1.DeviceModelNumber = "DBI8557MIMXXLBS";
            johnApartamentDevice1.DeviceSerialNumber = "131002350324";
            johnApartamentDevice1.IpAdress = "228.215.193.124";
            johnApartamentDevice1.MacAdrress = "87-86-1F-E0-A3-A6";
            johnApartamentDevice1.DeviceProduser = "Asko";
            johnApartamentDevice1.ManualBookLink = "https://www.bruksanvisni.ng/asko/dbi8557mimxxls/bruksanvisning";
            

            // Device Warranty 1 

            DeviceWarranty johnAprtmntDevice1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraDeviceInsuranceLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf"
            };
            int johnAprtmntDevice1WarrantyPeriod = 2;
            johnAprtmntDevice1Warranty.ExtraDeviceInsuranceLength = 3;
            DateTime purchaseDate3 = new DateTime(2022, 6, 10);
            DateTime validUntil3 = purchaseDate1.AddYears(johnAprtmntDevice1WarrantyPeriod + jamesDevice2Warranty.ExtraDeviceInsuranceLength);
            johnAprtmntDevice1Warranty.PurchaseDate = purchaseDate3;
            now = DateTime.Now;
            johnAprtmntDevice1Warranty.WarrantyPeriod = validUntil3 - now;
            johnApartamentDevice1.DeviceWarranty = johnAprtmntDevice1Warranty;

            // Shop 1

            Shop shop3 = new Shop();
            shop3.ShopName = "Elgiganten";
            shop3.PhoneNumber = 0771115115;
            shop3.ShopWebAdrress = "https://www.elgiganten.se/";
            johnAprtmntDevice1Warranty.Shop = shop3;

            // Shop 1 Address

            Address shop3Address = new Address();
            shop3Address.StreetName = "1007 N Main St";
            shop3Address.HouseNumber = 110;
            shop3Address.City = "Logan";
            shop3Address.Country = "USA";
            johnAprtmntDevice1Warranty.Shop.Adrress = shop3Address;
            
            // Adding Device object to the RealEstate list
            johnHouse1.DevicesProfile.Add(johnApartamentDevice1);

            // Adding RealEstate object to the userProfile List
            userProfile2.House.Add(johnHouse1);

            // Adding userProfile to the user List
            usersList.Add(userProfile2);


            return usersList;
        }
        
    }
}
