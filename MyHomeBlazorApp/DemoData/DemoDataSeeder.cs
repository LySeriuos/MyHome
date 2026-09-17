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

        // Don't accidentally create the demo data twice
        if (user.UserProfile.RealEstates.Any())
            throw new Exception("User already has real estate data.");

        var familyHouse = CreateFamilyHouse();
        var apartment = CreateApartment();
        var summerHouse = CreateSummerHouse();

        user.UserProfile.RealEstates.Add(familyHouse);
        user.UserProfile.RealEstates.Add(apartment);
        user.UserProfile.RealEstates.Add(summerHouse);

        await _dbContext.SaveChangesAsync();
    }

    private RealEstate CreateFamilyHouse()
    {
        var property = new RealEstate
        {
            RealEstateName = "Family House",

            Address = new Address
            {
                StreetName = "Oak Street",
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
                1));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Washing Machine",
                "Bosch",
                "WGG244Z0SN",
                DeviceType.Cleaning,
                new DateTime(2025, 6, 10),
                2));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Cooker Hood",
                "Siemens",
                "LR97CAQ50",
                DeviceType.Kitchen,
                new DateTime(2025, 12, 17),
                2));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Robot Vacuum",
                "Roborock",
                "S8 MaxV Ultra",
                DeviceType.Cleaning,
                new DateTime(2026, 3, 12),
                2));

        return property;
    }

    private RealEstate CreateApartment()
    {
        var property = new RealEstate
        {
            RealEstateName = "City Apartment",

            Address = new Address
            {
                StreetName = "Central Street",
                HouseNumber = 18,
                ApartamentNumber = 42,
                City = "Lund",
                Country = "Sweden"
            }
        };

        property.DevicesProfiles.Add(
            CreateDevice(
                "Television",
                "LG",
                "OLED55C4",
                DeviceType.Video,
                new DateTime(2026, 1, 14),
                2));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Laptop",
                "Lenovo",
                "ThinkPad T14",
                DeviceType.Computer,
                new DateTime(2025, 11, 5),
                3));

        return property;
    }

    private RealEstate CreateSummerHouse()
    {
        var property = new RealEstate
        {
            RealEstateName = "Summer House",

            Address = new Address
            {
                StreetName = "Beach Road",
                HouseNumber = 7,
                City = "Ystad",
                Country = "Sweden"
            }
        };

        property.DevicesProfiles.Add(
            CreateDevice(
                "Lawn Mower",
                "Husqvarna",
                "Automower 310 Mark II",
                DeviceType.Garden,
                new DateTime(2025, 4, 20),
                2));

        property.DevicesProfiles.Add(
            CreateDevice(
                "Security Camera",
                "Reolink",
                "RLC-811A",
                DeviceType.Security,
                new DateTime(2026, 5, 2),
                2));

        return property;
    }

    private DeviceProfile CreateDevice(
        string name,
        string producer,
        string model,
        DeviceType type,
        DateTime purchaseDate,
        int warrantyYears)
    {
        return new DeviceProfile
        {
            DeviceName = name,
            DeviceProduser = producer,
            DeviceModelNumber = model,
            DeviceSerialNumber =
                $"DEMO-{Guid.NewGuid().ToString()[..8].ToUpper()}",

            DeviceType = type,

            DeviceWarranty = new DeviceWarranty
            {
                PurchaseDate = purchaseDate,
                Years = warrantyYears,

                Shop = new Shop
                {
                    ShopName = "Demo Electronics",
                    ShopWebAddress = "https://example.com",

                    Address = new Address
                    {
                        StreetName = "Market Street",
                        HouseNumber = 10,
                        City = "Malmö",
                        Country = "Sweden"
                    }
                }
            }
        };
    }
}
