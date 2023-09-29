using MyHome;

namespace My_Home
{
    public static class TestData
    {

        public static List<UserProfile> Users()
        {
            List<UserProfile> usersList = new List<UserProfile>();

            // Users Profile 1
            #region User1 
            UserProfile userProfile1 = new UserProfile()
            {
                UserID = 1,
                UserName = "James",
                Email = "james@gmail.com",
                RealEstates = new List<RealEstate>()
            };
            // missing pasword method
           

            //Real Estate 1, Users can have more than one place

            RealEstate jamesRealEstate1 = new RealEstate()
            {
                RealEstateID = 1,
                RealEstateName = "James RealEstates",
                DevicesProfiles = new List<DeviceProfile>()
            };

            // Adding RealEstate object to the userProfile List

            userProfile1.RealEstates.Add(jamesRealEstate1);

            //Address RealEstate 1

            Address jamesRealEstate1Address = new Address()
            {
                StreetName = "Walton Street",
                HouseNumber = 583,
                City = "Salt Lake City",
                Country = "Usa"
            };

            jamesRealEstate1.Address = jamesRealEstate1Address;

            // DevicesProfiles 1

            DeviceProfile jamesDevice1 = new DeviceProfile()
            {
                DeviceID = 1,
                DeviceName = "Gaming Computer",
                DeviceType = DeviceType.Computer,
                DeviceModelNumber = "NPC-I7-4070-0423",
                DeviceSerialNumber = "7394291106279",
                IpAddress = "58.31.187.181",
                MacAdrress = "2E-A2-C8-A8-20-99",
                DeviceProduser = "Samsung",
                ManualBookLink = "https://www.bhphotovideo.com/lit_files/116441.pdf"
            };

            // Adding Device object to the RealEstate list

            jamesRealEstate1.DevicesProfiles.Add(jamesDevice1);


            // Device Warranty 1

            DeviceWarranty jamesDevice1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://discuss.poynt.net/uploads/default/original/2X/6/60c4199364474569561cba359d486e6c69ae8cba.jpeg",
                ExtraInsuranceWarrantyLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf",
                WarrantyPeriod = new TimeSpan(365, 0, 0, 0, 0),
                ExtraInsuranceWarrantyLenght = new TimeSpan(1095, 0, 0, 0),
                PurchaseDate = new DateTime(2023, 7, 15),
            };

            jamesDevice1.DeviceWarranty = jamesDevice1Warranty;

            // Shop 1

            Shop shop1 = new Shop()
            {
                ShopName = "Tesco",
                PhoneNumber = 00441992632222,
                ShopWebAddress = "https://www.tescoplc.com/"
            };

            jamesDevice1Warranty.Shop = shop1;

            // Shop Address 1

            Address shopAddress1 = new Address()
            {
                StreetName = "Indiana Ave",
                HouseNumber = 1925,
                City = "Salt Lake City",
                Country = "USA"
            };

            jamesDevice1Warranty.Shop.Address = shopAddress1;

            // DevicesProfiles 2

            DeviceProfile jamesDevice2 = new DeviceProfile()
            {
                DeviceID = 2,
                DeviceName = "James Phone",
                DeviceType = DeviceType.MobileDevice,
                DeviceModelNumber = "A2891",
                DeviceSerialNumber = "HM95O92P2F",
                IpAddress = "192.168.1.13",
                MacAdrress = "00-B0-D0-63-C2-26",
                DeviceProduser = "Apple",
                ManualBookLink = "https://support.apple.com/sv-se/guide/iphone/iphfc2d9bc6a/ios"
            };

            // Adding Device object to the RealEstate list

            jamesRealEstate1.DevicesProfiles.Add(jamesDevice2);

            // Device Warranty 2 

            DeviceWarranty jamesDevice2Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraInsuranceWarrantyLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf",
                WarrantyPeriod = new TimeSpan(365, 0, 0, 0, 0),
                ExtraInsuranceWarrantyLenght = new TimeSpan(365, 0, 0, 0, 0),
                PurchaseDate = new DateTime(2023, 1, 23)
            };

            jamesDevice2.DeviceWarranty = jamesDevice2Warranty;

            // Shop 2

            Shop shop2 = new Shop()
            {
                ShopName = "T-Mobile",
                PhoneNumber = 18009378997,
                ShopWebAddress = "https://www.t-mobile.com/?INTNAV=tNav:Home",
            };

            jamesDevice2Warranty.Shop = shop2;

            // Shop 2 Address

            Address shop2Address = new Address()
            {
                StreetName = "1007 N Main St",
                HouseNumber = 110,
                City = "Logan",
                Country = "USA"
            };

            jamesDevice2Warranty.Shop.Address = shop2Address;

            // Adding userProfile to the user List
            usersList.Add(userProfile1);


            #endregion
            ////////// Users Profile 2 ///////////////////////
            #region User2

            UserProfile userProfile2 = new UserProfile()
            {
                UserID = 2,
                UserName = "John",
                Email = "john@gmail.com",
                RealEstates = new List<RealEstate>()
            };
            // missing pasword method

            //Place name 1
            RealEstate johnHouse1 = new RealEstate()
            {
                RealEstateID = 1,
                RealEstateName = "John Apartament",
                DevicesProfiles = new List<DeviceProfile>()
            };

            // Adding RealEstate object to the userProfile List
            userProfile2.RealEstates.Add(johnHouse1);

            //Address Realestate 1

            Address johnHouse1Address = new Address()
            {
                StreetName = "Valldammsgatan",
                HouseNumber = 96,
                City = "Gusum",
                Country = "Sweden"
            };

            johnHouse1.Address = johnHouse1Address;

            // maybe I should put State, it could be useful

            // DevicesProfiles 1

            DeviceProfile johnHouse1Device1 = new DeviceProfile()
            {
                DeviceID = 1,
                DeviceName = "DishWasher",
                DeviceType = DeviceType.Kitchen,
                DeviceModelNumber = "DBI8557MIMXXLBS",
                DeviceSerialNumber = "131002350324",
                IpAddress = "228.215.193.124",
                MacAdrress = "87-86-1F-E0-A3-A6",
                DeviceProduser = "Asko",
                ManualBookLink = "https://www.bruksanvisni.ng/asko/dbi8557mimxxls/bruksanvisning"
            };

            // Adding Device object to the RealEstate list
            johnHouse1.DevicesProfiles.Add(johnHouse1Device1);

            // Device Warranty 1 

            DeviceWarranty johnHouse1Device1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://img.freepik.com/free-vector/receipt-template-collection-with-realistic-design_23-2147910552.jpg?w=1380&t=st=1692650469~exp=1692651069~hmac=91e29da374bc7eddec6ec6c10e9cb828b93ac76cf4c79d558a6ea1257ef728a4",
                ExtraInsuranceWarrantyLink = "https://img.yumpu.com/20322498/1/500x640/ladda-ned-produktblad-som-pdf.jpg",
                WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0),
                ExtraInsuranceWarrantyLenght = new TimeSpan(1095, 0, 0, 0, 0),
                PurchaseDate = new DateTime(2022, 6, 10)
            };

            johnHouse1Device1.DeviceWarranty = johnHouse1Device1Warranty;

            // Shop 1

            Shop shop3 = new Shop()
            {
                ShopName = "Elgiganten",
                PhoneNumber = 0771115115,
                ShopWebAddress = "https://www.elgiganten.se/"
            };

            johnHouse1Device1Warranty.Shop = shop3;

            // Shop 1 Address

            Address shop3Address = new Address()
            {
                StreetName = "Norra Svedengatan",
                HouseNumber = 19,
                City = "Linköping",
                Country = "Sweden"
            };
            johnHouse1Device1Warranty.Shop.Address = shop3Address;


            //Place name 2

            RealEstate johnHouse2 = new RealEstate()
            {
                RealEstateID = 2,
                RealEstateName = "John RealEstate Rental",
                DevicesProfiles = new List<DeviceProfile>()
            };

            //Address Realestate 2

            Address johnHouse2Address = new Address()
            {
                StreetName = "Vipgränden",
                HouseNumber = 81,
                HouseNumberExtension = "A",
                City = "Gullringen",
                Country = "Sweden"
            };

            johnHouse2.Address = johnHouse2Address;
            // maybe I should put State, it could be useful

            // Realestate 2 DevicesProfiles 1

            DeviceProfile johnHouse2Device1 = new DeviceProfile()
            {
                DeviceID = 2,
                DeviceName = "Washing-Drying Machine",
                DeviceType = DeviceType.Cleaning, // could be gruoped as multidevice but still it should be grouped as Cleaning device
                DeviceModelNumber = "CV90V6S2BA",
                DeviceSerialNumber = "1ABC2DEF345678G",
                IpAddress = "188.188.72.36",
                MacAdrress = "F8-15-50-CC-A3-6E",
                DeviceProduser = "LG",
                // local file
                ManualBookLink = "file:///C:/Users/shiranco.DESKTOP-HRN41TE/Downloads/[EPREL_20230519175627742_20230519175627742]%20(1).pdf"
            };


            // Realestate 2 Device1 Warranty 

            DeviceWarranty johnHouse2Device1Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://studylib.net/doc/25904030/t-mobile-receipt-en-edited.pdf",
                ExtraInsuranceWarrantyLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf",
                WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0),
                ExtraInsuranceWarrantyLenght = new TimeSpan(365, 0, 0, 0, 0),
                PurchaseDate = new DateTime(2021, 1, 14)
            };

            johnHouse2Device1.DeviceWarranty = johnHouse2Device1Warranty;

            // Realestate 2 Shop 1

            Shop shop4 = new Shop()
            {
                ShopName = "Elgiganten",
                PhoneNumber = 0771115115,
                ShopWebAddress = "https://www.elgiganten.se/",
            };

            johnHouse2Device1Warranty.Shop = shop4;

            // Realestate 2 Shop 1 Address

            Address shop4Address = new Address()
            {
                StreetName = "Ljunghedsvägen",
                HouseNumber = 7,
                City = "Västervik",
                Country = "Sweden"
            };

            johnHouse2Device1Warranty.Shop.Address = shop4Address;

            // Adding Device object to the RealEstate list
            johnHouse2.DevicesProfiles.Add(johnHouse2Device1);

            // Realestate 2 DevicesProfiles 2

            DeviceProfile johnHouse2Device2 = new DeviceProfile()
            {
                DeviceID = 3,
                DeviceName = "Main Bedroom TV",
                DeviceType = DeviceType.Video,
                DeviceModelNumber = "TQ55LS03BGUXXC",
                DeviceSerialNumber = "AKZ43CPQ505923D",
                IpAddress = "134.121.255.219",
                MacAdrress = "B5-76-8F-F6-7A-1B",
                DeviceProduser = "Samsung",
                ManualBookLink = "https://www.samsung.com/uk/support/model/QE55LS03BGUXXU/#downloads"
            };


            // Realestate 2 Device 2 Warranty  

            DeviceWarranty johnHouse2Device2Warranty = new DeviceWarranty
            {
                ReceiptLink = "https://qph.cf2.quoracdn.net/main-qimg-8aa597ee2d773fce151545846c9d08a0-pjlq",
                ExtraInsuranceWarrantyLink = "https://www.stewart.com/content/dam/stewart/Microsites/mexico/pdfs/01_20_2023-intl-stgm-title-app-corporation-english.pdf",
                WarrantyPeriod = new TimeSpan(730, 0, 0, 0, 0),
                ExtraInsuranceWarrantyLenght = new TimeSpan(365, 0, 0, 0, 0),
                PurchaseDate = new DateTime(2021, 3, 23)
            };

            johnHouse2Device2.DeviceWarranty = johnHouse2Device2Warranty;


            // Realestate 2 Shop 2

            Shop shop5 = new Shop()
            {
                ShopName = "Elgiganten",
                PhoneNumber = 0771115115,
                ShopWebAddress = "https://www.elgiganten.se/"
            };
            johnHouse2Device2Warranty.Shop = shop5;

            // Real Estate 2 Shop 2 Address

            // if it is the same shop?
            Address shop5Address = new Address()
            {
                StreetName = "Ljunghedsvägen",
                HouseNumber = 7,
                City = "Västervik",
                Country = "Sweden"
            };

            johnHouse2Device2Warranty.Shop.Address = shop5Address;

            // Adding Device object to the RealEstate list
            johnHouse2.DevicesProfiles.Add(johnHouse2Device2);

            // Adding RealEstate object to the userProfile List
            userProfile2.RealEstates.Add(johnHouse2);
            #endregion
            // Adding userProfile to the user List
            usersList.Add(userProfile2);

            return usersList;
        }

    }
}
