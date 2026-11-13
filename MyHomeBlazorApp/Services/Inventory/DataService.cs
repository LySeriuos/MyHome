using MyHomeBlazorApp.Services.Files;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using My_Home;
using MyHome;
using MyHome.Models;
using System.Xml;
//using System.Data.Entity;

namespace MyHomeBlazorApp.BlazorData

{

    public partial class DataService
    {
        public DataService(MyHomeBlazorAppContext dbcontext, UserManager<MyHomeBlazorAppUser> usermanager, AuthenticationStateProvider authenticationStateProvider, DeviceFileStorage fileStorage)
        {
            _userManager = usermanager;
            //_users = Data.GetUsersListFromXml(_path);
            _authenticationStateProvider = authenticationStateProvider;
            _dbcontext = dbcontext;
            _fileStorage = fileStorage;
        }

        private readonly DeviceFileStorage _fileStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private UserManager<MyHomeBlazorAppUser> _userManager;
        private MyHomeBlazorAppContext _dbcontext;
        private UserProfile? _currentUserWithAllData;
        public UserProfile CurrentUserWithAllData => _currentUserWithAllData ?? new UserProfile();
        public MyHomeBlazorAppUser? CurrentAppUser { get; set; }
        public List<DeviceProfile>? Devices => _currentUserWithAllData?.GetAllDevices();
        public List<DeviceProfile>? expiringDeviceDevices { get; set; } = new List<DeviceProfile>();
        public DeviceProfile? FirstExpiringDevice { get; set; } = new DeviceProfile();
        public List<DeviceWarranty>? DevicesWarranties { get; set; } = new List<DeviceWarranty>();
        public DeviceProfile? CurrentDevice { get; set; } = new DeviceProfile();
        public List<DeviceProfile>? UnassignedDevicesList { get; set; }
        public List<DeviceProfile> SelectedDevicesListToPrintQrCodes { get; set; } = new();
        public MyHomeBlazorAppContext DbContext => _dbcontext;

        public Task UpdateObjectInDB() => _dbcontext.SaveChangesAsync();
    }
}
