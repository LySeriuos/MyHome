using Microsoft.EntityFrameworkCore;
using MyHome.Models;
using MyHomeBlazorApp.BlazorData;

namespace MyHomeBlazorApp.DemoData;

public class DemoDataSeeder
{
    private readonly MyHomeBlazorAppContext _dbContext;

    public DemoDataSeeder(MyHomeBlazorAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedDemoDataAsync(string userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserProfile!)
                .ThenInclude(p => p.RealEstates)
            .Include(u => u.UserProfile!)
                .ThenInclude(p => p.UnassignedDevicesList)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new Exception("User not found.");

        if (user.UserProfile == null)
        {
            user.UserProfile = new UserProfile
            {
                UserName = user.UserName ?? "Demo User",
                Email = user.Email ?? string.Empty
            };
        }

        // Prevent accidentally loading the demo data more than once.
        if (user.UserProfile.RealEstates.Any())
            throw new Exception("User already has real estate data.");

        user.UserProfile.RealEstates.Add(CreateFamilyHouse());
        user.UserProfile.RealEstates.Add(CreateCityApartment());
        user.UserProfile.RealEstates.Add(CreateSummerHouse());

        await _dbContext.SaveChangesAsync();
    }

    // =========================================================
    // FAMILY HOUSE - 9 devices
    // =========================================================

    private RealEstate CreateFamilyHouse()
    {
        var property = new RealEstate
        {
            RealEstateName = "Family House",

            // Fictional residential address
            Address = new Address
            {
                StreetName = "Ekvägen",
                HouseNumber = 24,
                City = "Malmö",
                Country = "Sweden"
            }
        };

        property.DevicesProfiles.Add(
            CreateDevice(
                "Dishwasher",
                "Bosch",
                "SMV6ZCX10E",
                DeviceType.Kitchen,
                new DateTime(2025, 10, 15),
                1,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Washing Machine",
                "Bosch",
                "WGG244Z0SN",
                DeviceType.Cleaning,
                new DateTime(2025, 6, 10),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Tumble Dryer",
                "Bosch",
                "WQG245DASN",
                DeviceType.Cleaning,
                new DateTime(2025, 6, 10),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Cooker Hood",
                "Siemens",
                "LR97CAQ50",
                DeviceType.Kitchen,
                new DateTime(2025, 12, 17),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Oven",
                "Siemens",
                "HB774G1B1",
                DeviceType.Kitchen,
                new DateTime(2025, 12, 17),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Robot Vacuum",
                "Roborock",
                "S8 MaxV Ultra",
                DeviceType.Cleaning,
                new DateTime(2026, 3, 12),
                2,
                CreateNetOnNet()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Television",
                "LG",
                "OLED65C4",
                DeviceType.Video,
                new DateTime(2026, 1, 14),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Wi-Fi Router",
                "TP-Link",
                "Archer AX55",
                DeviceType.Computer,
                new DateTime(2026, 2, 8),
                2,
                CreateNetOnNet()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Wireless Speaker",
                "Sonos",
                "Era 100",
                DeviceType.Audio,
                new DateTime(2026, 4, 18),
                2,
                CreateElgiganten()));

        return property;
    }

    // =========================================================
    // CITY APARTMENT - 5 devices
    // =========================================================

    private RealEstate CreateCityApartment()
    {
        var property = new RealEstate
        {
            RealEstateName = "City Apartment",

            // Fictional residential address
            Address = new Address
            {
                StreetName = "Centralgatan",
                HouseNumber = 18,
                ApartamentNumber = 42,
                City = "Lund",
                Country = "Sweden"
            }
        };

        property.DevicesProfiles.Add(
            CreateDevice(
                "Television",
                "Samsung",
                "QE55S90D",
                DeviceType.Video,
                new DateTime(2026, 2, 5),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Laptop",
                "Lenovo",
                "ThinkPad T14",
                DeviceType.Computer,
                new DateTime(2026, 1, 20),
                3,
                CreateNetOnNet()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Coffee Machine",
                "De'Longhi",
                "Magnifica Evo",
                DeviceType.Kitchen,
                new DateTime(2026, 3, 15),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Soundbar",
                "Sonos",
                "Beam Gen 2",
                DeviceType.Audio,
                new DateTime(2026, 4, 2),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Air Purifier",
                "Philips",
                "Series 3000i",
                DeviceType.Other,
                new DateTime(2026, 5, 11),
                2,
                CreateNetOnNet()));

        return property;
    }

    // =========================================================
    // SUMMER HOUSE - 3 devices
    // =========================================================

    private RealEstate CreateSummerHouse()
    {
        var property = new RealEstate
        {
            RealEstateName = "Summer House",

            // Fictional residential address
            Address = new Address
            {
                StreetName = "Strandvägen",
                HouseNumber = 7,
                City = "Ystad",
                Country = "Sweden"
            }
        };

        property.DevicesProfiles.Add(
            CreateDevice(
                "Refrigerator",
                "Electrolux",
                "LRS4DF18S",
                DeviceType.Kitchen,
                new DateTime(2026, 3, 21),
                2,
                CreateElgiganten()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Robot Lawn Mower",
                "Husqvarna",
                "Automower 310 Mark II",
                DeviceType.Garden,
                new DateTime(2026, 4, 12),
                2,
                CreateNetOnNet()));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Security Camera",
                "Reolink",
                "RLC-811A",
                DeviceType.Security,
                new DateTime(2026, 6, 15),
                2,
                CreateNetOnNet()));

        return property;
    }

    // =========================================================
    // DEVICE
    // =========================================================

    private DeviceProfile CreateDevice(
        string name,
        string producer,
        string model,
        DeviceType type,
        DateTime purchaseDate,
        int warrantyYears,
        Shop shop)
    {
        return new DeviceProfile
        {
            DeviceName = name,

            // Real manufacturer + model so MyHome's
            // manual-search feature returns useful results.
            DeviceProduser = producer,
            DeviceModelNumber = model,

            // Fictional serial number.
            DeviceSerialNumber =
                $"DEMO-{Guid.NewGuid().ToString()[..8].ToUpper()}",

            DeviceType = type,

            // ManualBookLink intentionally not populated.
            // MyHome searches for the manual using producer + model.

            DeviceWarranty = new DeviceWarranty
            {
                PurchaseDate = purchaseDate,
                Years = warrantyYears,
                ExtendedWarrantyinYears = 0,
                Shop = shop
            }
        };
    }

    // =========================================================
    // REAL RETAILERS
    // =========================================================

    private Shop CreateElgiganten()
    {
        return new Shop
        {
            ShopName = "Elgiganten Svågertorp",
            ShopWebAddress = "https://www.elgiganten.se",
            PhoneNumber = 0,

            Address = new Address
            {
                StreetName = "Nornegatan",
                HouseNumber = 12,
                City = "Malmö",
                Country = "Sweden"
            }
        };
    }

    private Shop CreateNetOnNet()
    {
        return new Shop
        {
            ShopName = "NetOnNet Malmö Svågertorp",
            ShopWebAddress = "https://www.netonnet.se",
            PhoneNumber = 0,

            Address = new Address
            {
                StreetName = "Nornegatan",
                HouseNumber = 5,
                City = "Malmö",
                Country = "Sweden"
            }
        };
    }
}