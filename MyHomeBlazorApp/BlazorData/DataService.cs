using java.security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using My_Home;
using MyHome;
using MyHome.Models;
using sun.invoke.empty;
using sun.security.jca;
using System.Xml;
//using System.Data.Entity;


namespace MyHomeBlazorApp.BlazorData

{

    public class DataService
    {
        public DataService(MyHomeBlazorAppContext dbcontext, UserManager<MyHomeBlazorAppUser> usermanager, AuthenticationStateProvider authenticationStateProvider)
        {
            _userManager = usermanager;
            //_users = Data.GetUsersListFromXml(_path);
            _authenticationStateProvider = authenticationStateProvider;
            _dbcontext = dbcontext;
        }

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

        #region User

        public async Task<MyHomeBlazorAppUser?> GetUserWithProfileAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var principal = authState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(principal);

                // This 'Include' is the ONLY reason why .UserProfile won't be null!
                return await _dbcontext.Users
                    .Include(u => u.UserProfile)
                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
            return null;
        }


        /// <summary>
        /// Get Authenticated User from Identity system and load the corresponding UserProfile from the database.
        /// </summary>
        /// <returns>Logged in user as current application user.</returns>
        public async Task<MyHomeBlazorAppUser?> GetAuthenticatedUserAsync()
        {
            //Check cache first
            if (!string.IsNullOrEmpty(CurrentAppUser?.Id))
            {
                return CurrentAppUser;
            }
            // Get the identity from the browser session
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var principal = authState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                // Fetch the real user from DB
                CurrentAppUser = await _userManager.GetUserAsync(principal);
            }

            return CurrentAppUser;
        }

        /// <summary>
        /// Preloads the authenticated user and their full UserProfile with all related data. This method should be called at the start of the application to ensure that all user-related data is available for subsequent operations. It checks if the user is already loaded to avoid redundant database calls. If the user is not authenticated or if any issues arise, it will log a message and return without loading any data.
        /// </summary>
        /// <returns></returns>
        public async Task InitializedUserAsync()
        {
            if (_currentUserWithAllData?.UserID != 0 && _currentUserWithAllData != null)
            {
                return;
            }


            await GetAuthenticatedUserAsync();

            if (CurrentAppUser == null)
            {
                Console.WriteLine("DataService: No Authenticated User found.");
                return;
            }

            await LoadUserWithAllDataAsync();

            if (_currentUserWithAllData != null)
            {
                expiringDeviceDevices = Logic.ExpiringDevicesWarrantiesInDays(_currentUserWithAllData, 180);
                FirstExpiringDevice = FirstexpiringDeviceWarranty();
                DevicesWarranties = Logic.GetUserDevicesWarranties(_currentUserWithAllData);
            }
            else
            {
                // "Else" Case: Reset these to empty states so the UI doesn't show old data
                expiringDeviceDevices = new List<DeviceProfile>();
                DevicesWarranties = new List<DeviceWarranty>();
                FirstExpiringDevice = null;
            }
        }

        /// <summary>
        /// Loads the authenticated user along with all related data (UserProfile, RealEstates, DevicesProfiles, DeviceWarranties, Shops, and Addresses) in a single database query. This method is designed to minimize database round-trips and improve performance by using eager loading with Include and ThenInclude. It also uses AsSplitQuery to handle large collections efficiently. If the user is not authenticated or if any issues arise during the loading process, it will log a message and return without loading any data.
        /// </summary>
        /// <returns>User with all the data</returns>
        public async Task LoadUserWithAllDataAsync()
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return;

            // The "Master Query": One trip to the DB for everything
            var fullUser = await _dbcontext.Users
    .Include(u => u.UserProfile!)
        .ThenInclude(p => p.UnassignedDevicesList)
            .ThenInclude(d => d.DeviceWarranty)
                .ThenInclude(w => w.Shop)
                    .ThenInclude(s => s.Address)
    .Include(u => u.UserProfile!)
        .ThenInclude(p => p.RealEstates)
            .ThenInclude(r => r.Address)
    .Include(u => u.UserProfile!)
        .ThenInclude(p => p.RealEstates)
            .ThenInclude(r => r.DevicesProfiles)
                .ThenInclude(d => d.DeviceWarranty)
                    .ThenInclude(w => w.Shop)
                        .ThenInclude(s => s.Address)
                        .AsSplitQuery() // Tells EF Core to load collections cleanly in parallel queries
    .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (fullUser != null)
            {
                CurrentAppUser = fullUser;
                // Update your helper field
                _currentUserWithAllData = fullUser.UserProfile;
            }
        }

        /// <summary>
        /// Asynchronously retrieves the list of unassigned device profiles associated with the currently authenticated
        /// user.
        /// </summary>
        /// <remarks>If no user is currently authenticated, the method attempts to authenticate the user
        /// before retrieving the unassigned devices. The returned list is never null.</remarks>
        /// <returns>A list of <see cref="DeviceProfile"/> objects representing devices that are unassigned for the current user.
        /// Returns an empty list if no user is authenticated or if there are no unassigned devices.</returns>
        public async Task<List<DeviceProfile>> GetUserWithUnassignedDevicesListAsync()
        {
            //Ensuring that user is logged in 
            if (CurrentAppUser == null)
            {
                await GetAuthenticatedUserAsync();
            }
            // if user is not logged in just return empty list
            if (CurrentAppUser == null)
            {
                return new List<DeviceProfile>();
            }

            var userWithData = await _dbcontext.Users
                .Include(u => u.UserProfile!)
                .ThenInclude(u => u.UnassignedDevicesList)
                .FirstOrDefaultAsync(u => u.Id == CurrentAppUser.Id);

            UnassignedDevicesList = userWithData?.UserProfile?.UnassignedDevicesList?.ToList() ?? new List<DeviceProfile>();

            return UnassignedDevicesList;
        }
        /// <summary>
        /// Checks if valid data exists for the authenticated user. If not, it attempts to load the user with all related data. This method is useful for ensuring that the application has the necessary user data before performing operations that depend on it.
        /// </summary>
        /// <returns>Loads the user data with all related information if available; otherwise, throws an exception.</returns>
        /// <exception cref="InvalidOperationException">Thrown when user data could not be loaded.</exception>
        private async Task EnsureUserDataLoadedAsync()
        {
            if (_currentUserWithAllData == null || _currentUserWithAllData.UserID == 0)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData == null || _currentUserWithAllData.UserID == 0)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }
        }

        /// <summary>
        /// Extra check if device is owned by user. This is a security measure to ensure that users can only access devices they own, either through their real estates or their unassigned devices list.
        /// </summary>
        /// <param name="deviceId">The ID of the device to check ownership for.</param>
        /// <param name="identityUserId">The ID of the user to check against.</param>
        /// <returns>True if the device is owned by the user; otherwise, false.</returns>
        public async Task<bool> IsDeviceOwnedByUserAsync(int deviceId, string identityUserId)
        {
            // Check if the user owns the device, either in userProfile.RealEstates or in userProfile.UnassignedDevicesList
            return await _dbcontext.Users
                .AnyAsync(u => u.Id == identityUserId && (
                    // Pathway 1: The device is linked to one of the user's real estates
                    u.UserProfile!.RealEstates.Any(r => r.DevicesProfiles.Any(d => d.DeviceID == deviceId))
                    ||
                    // Pathway 2: The device is in the user's unassigned list
                    u.UserProfile.UnassignedDevicesList.Any(d => d.DeviceID == deviceId)
                ));
        }

        #endregion

        #region Real Estate

        /// <summary>
        /// Get RealEstate object by realEstate ID
        /// </summary>
        /// <param name="realEstateID">RealEstate ID to look for</param>
        /// <returns>RealEstate object</returns>
        public RealEstate? GetRealEstate(int realEstateID)
        {
            if (_currentUserWithAllData?.RealEstates == null) return null;

            var foundRealEstate = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateID);
            if (foundRealEstate != null)
            {
                return foundRealEstate;
            }
            return null;
        }

        /// <summary>
        /// Method to save created new RealEstate
        /// </summary>
        /// <param name="realEstate">Created RealEstate</param>


        public async Task AddNewRealEstateToDBAsync(RealEstate currentRealEstate)
        {
            if (_currentUserWithAllData == null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData == null)
            {
                throw new InvalidOperationException("User data must be loaded before adding a real estate.");
            }

            if (currentRealEstate.RealEstateID != 0)
            {
                throw new ArgumentException("Wrong RealEstate ID", nameof(currentRealEstate.RealEstateID));
            }

            _currentUserWithAllData.RealEstates.Add(currentRealEstate);
        }

        /// <summary>
        /// Method to get RealEstate object by Device ID parameter
        /// </summary>
        /// <param name="deviceId">Parameter</param>
        /// <returns>RealEstate's ID</returns>
        public int GetRealEstateByDeviceID(int deviceId)
        {
            if (_currentUserWithAllData?.RealEstates == null) return 0;
            var foundRealEstate = _currentUserWithAllData.RealEstates.FirstOrDefault(re => re.DevicesProfiles.Any(d => d.DeviceID == deviceId));
            if (foundRealEstate != null)
            {
                return foundRealEstate.RealEstateID;
            }
            return 0;
        }



        /// <summary>
        /// To get last added RealEstate in RealEstates list
        /// </summary>
        /// <returns>Last added RealEstate in the list</returns>        
        public RealEstate? LastAddedRealEstate()
        {
            return _currentUserWithAllData?.RealEstates.LastOrDefault();
        }


        /// <summary>
        /// Method to delete(remove) RealEstate from the RealEstates list
        /// </summary>
        /// <param name="contextChosedRealEstateID">Chosed RealEstate</param>
        public async Task RemoveRealEstateFromDb(int realEstateId)
        {
            var sourceRealEstate = await GetRealEstateWithAllData(realEstateId);

            if (sourceRealEstate == null) return;

            if (sourceRealEstate.Address != null)
            {
                _dbcontext.Remove(sourceRealEstate.Address);
            }

            if (sourceRealEstate.DevicesProfiles != null)
            {
                foreach (var device in sourceRealEstate.DevicesProfiles.ToList())
                {
                    if (device.DeviceWarranty?.Shop?.Address is { } shopAddress)
                        _dbcontext.Remove(shopAddress);

                    if (device.DeviceWarranty?.Shop is { } shop)
                        _dbcontext.Remove(shop);

                    if (device.DeviceWarranty is { } warranty)
                        _dbcontext.Remove(warranty);

                    _dbcontext.Remove(device); // Physically deletes the device record
                }
            }

            _dbcontext.Remove(sourceRealEstate);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteRealEstateAndKeepUnassignedDevices(int realEstateId)
        {
            if (_currentUserWithAllData is null)
            {
                await LoadUserWithAllDataAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            var sourceRealEstate = await GetRealEstateWithAllData(realEstateId);

            if (sourceRealEstate == null)
            {
                return;
            }

            if (sourceRealEstate.DevicesProfiles != null)
            {
                foreach (var targetDevice in sourceRealEstate.DevicesProfiles.ToList())
                {
                    _currentUserWithAllData.UnassignedDevicesList.Add(targetDevice);
                    sourceRealEstate.DevicesProfiles.Remove(targetDevice);
                }
                await UpdateObjectInDB();
            }

            _dbcontext.Remove(sourceRealEstate);

            if (sourceRealEstate.Address != null)
            {
                _dbcontext.Remove(sourceRealEstate.Address);
            }

            sourceRealEstate.DevicesProfiles?.Clear();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteRealEstateAndReassignDevices(int sourceId, int targetRealEstateId)
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            var sourceRealEstate = await GetRealEstateWithAllData(sourceId);
            var targetRealEstate = await _dbcontext.Set<RealEstate>()
                .Include(r => r.DevicesProfiles)
                .FirstOrDefaultAsync(r => r.RealEstateID == targetRealEstateId);

            if (sourceRealEstate == null || targetRealEstate == null) return;

            if (sourceRealEstate.DevicesProfiles != null)
            {
                foreach (var device in sourceRealEstate.DevicesProfiles.ToList())
                {
                    sourceRealEstate.DevicesProfiles.Remove(device);
                    targetRealEstate.DevicesProfiles.Add(device);
                }
                await UpdateObjectInDB();
            }
            if (sourceRealEstate.Address != null)
            {
                _dbcontext.Remove(sourceRealEstate.Address);
            }

            _dbcontext.Remove(sourceRealEstate);
            _currentUserWithAllData.RealEstates.Remove(sourceRealEstate);
            await _dbcontext.SaveChangesAsync();
        }

        private Task<RealEstate?> GetRealEstateWithAllData(int id)
        {
            return _dbcontext.Set<RealEstate>()
                .Include(r => r.Address)
                .Include(r => r.DevicesProfiles)
                    .ThenInclude(d => d.DeviceWarranty)
                        .ThenInclude(w => w.Shop)
                            .ThenInclude(s => s.Address)
                .FirstOrDefaultAsync(r => r.RealEstateID == id);
        }


        #endregion
        #region Devices

        /// <summary>
        /// Method to add new device in to database
        /// </summary>
        /// <param name="deviceToAdd">Device object to be add</param>
        /// <param name="chosedRealEstateID">Real Estate Id to add new device into</param>
        /// <returns></returns>
        public async Task AddNewDeviceToDataBaseAsync(DeviceProfile deviceToAdd, int chosedRealEstateID)
        {
            await EnsureUserDataLoadedAsync();
            UserProfile currentUser = _currentUserWithAllData
    ?? throw new InvalidOperationException("User data could not be loaded.");
            deviceToAdd.DeviceWarranty ??= new();
            deviceToAdd.DeviceWarranty.Shop ??= new();
            deviceToAdd.DeviceWarranty.Shop.Address ??= new();
            deviceToAdd.TempRealEstateName = chosedRealEstateID.ToString();
            // if user has not created any real estates so the device will be added to unnassigned list
            if (chosedRealEstateID == 0)
            {
                _currentUserWithAllData.UnassignedDevicesList.Add(deviceToAdd);
            }
            // if user has real estate, device will be added to chosed real estate
            else
            {
                RealEstate? chosedRealEstate = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == chosedRealEstateID);

                if (chosedRealEstate == null)
                {
                    throw new Exception("Selected Real Estate not found.");
                }

                chosedRealEstate.DevicesProfiles.Add(deviceToAdd);
            }
        }

        public async Task<List<DeviceProfile>> GetAllUserDevicesAsync()
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }
            // Get the raw lists
            var assigned = _currentUserWithAllData.RealEstates
                .SelectMany(re => re.DevicesProfiles).ToList();
            var unassigned = _currentUserWithAllData.UnassignedDevicesList;

            var allDevices = assigned.Concat(unassigned).ToList();

            // Loop through and add realEstate ID or unnassigned based on if devices is assigned or not to the Real Estate
            foreach (var device in allDevices)
            {
                int realEstateId = GetRealEstateByDeviceID(device.DeviceID);

                if (realEstateId != 0)
                {   // combining real estate name and the id 
                    var realEstate = GetRealEstate(realEstateId);
                    device.TempRealEstateName = realEstate?.RealEstateName + " / " + realEstateId;
                }
                else
                {
                    // if real estate is not assigned it gives unnassigned and id 0
                    device.TempRealEstateName = "Unassigned / 0";
                }
            }
            return allDevices;
        }


        /// <summary>
        /// Method to remove device from DB
        /// </summary>
        /// <param name="deviceToDelete">Object to delete</param>
        /// <returns></returns>
        public async Task RemoveDeviceFromDb(DeviceProfile deviceToDelete)
        {
            // Query the specific Device directly from its own table with its exact child dependencies
            var fullDevice = await _dbcontext.Set<DeviceProfile>()
                .Include(d => d.DeviceWarranty)
                    .ThenInclude(w => w.Shop)
                        .ThenInclude(s => s.Address)
                .FirstOrDefaultAsync(d => d.DeviceID == deviceToDelete.DeviceID);

            if (fullDevice != null)
            {
                // Manually remove the child structures from the bottom up to clear the database safely
                if (fullDevice.DeviceWarranty != null)
                {
                    if (fullDevice.DeviceWarranty.Shop != null)
                    {
                        if (fullDevice.DeviceWarranty.Shop.Address != null)
                        {
                            _dbcontext.Remove(fullDevice.DeviceWarranty.Shop.Address);
                        }
                        _dbcontext.Remove(fullDevice.DeviceWarranty.Shop);
                    }
                    _dbcontext.Remove(fullDevice.DeviceWarranty);
                }

                // Remove the main device record
                _dbcontext.Remove(fullDevice);
                await UpdateObjectInDB();

            }
        }

        /// <summary>
        /// Getting last item in the sequence
        /// </summary>
        /// <returns>Returns last added device in the list</returns>
        public DeviceProfile? LastAddedDevice()
        {
            List<DeviceProfile>? devices = Devices;

            if (devices != null)
            {
                return devices.LastOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Method to move devices list from one RealEstate to another
        /// </summary>
        /// <param name="realEstateID">Devices list will be moved into RealEstate by realEstateID</param>
        /// <param name="currentRealEstate">RealEstate to move from(delete) devices list</param>
        public async Task MoveDevicesListToOtherRealEstate(int targetRealEstateId, RealEstate currentRealEstate)
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            RealEstate? targetRealEstate = _currentUserWithAllData.RealEstates
        .FirstOrDefault(r => r.RealEstateID == targetRealEstateId);
            if (targetRealEstate == null || currentRealEstate == null)
            {
                return;
            }

            List<DeviceProfile>? devicesToMove = currentRealEstate.DevicesProfiles.ToList();


            foreach (DeviceProfile deviceProfile in currentRealEstate.DevicesProfiles.ToList())
            {
                targetRealEstate.DevicesProfiles.Add(deviceProfile);
                currentRealEstate.DevicesProfiles.Remove(deviceProfile);
            }
            await UpdateObjectInDB();
        }

        /// <summary>
        /// Method to move DeviceProfile from one Real Estate to another
        /// </summary>
        /// <param name="deviceToMoveID">DeviceProfile ID which will be moved </param>
        /// <param name="_currentUserWithAllData">Indentified user</param>
        /// <param name="realEstateIdToAddDevice">Real Estate ID to move in Device by deviceToMoveID</param>
        public async Task MoveDeviceToOtherRealEstateAsync(int deviceToMoveID, int realEstateIdToAddDevice)
        {
            if (_currentUserWithAllData is null)
            {
                await InitializedUserAsync();
            }

            if (_currentUserWithAllData is null)
            {
                throw new InvalidOperationException("User data could not be loaded.");
            }

            int realEstateIdToMoveFrom = GetRealEstateByDeviceID(deviceToMoveID);
            DeviceProfile? deviceToMove = _currentUserWithAllData.RealEstates
        .SelectMany(r => r.DevicesProfiles)
        .FirstOrDefault(d => d.DeviceID == deviceToMoveID);

            RealEstate? realEstateToMoveFrom = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToMoveFrom);
            RealEstate? realEstateToAddDevice = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToAddDevice);
            if (realEstateToMoveFrom == null) return;
            if (realEstateToAddDevice != null)
            {
                realEstateToMoveFrom.DevicesProfiles.Remove(deviceToMove);
                realEstateToAddDevice.DevicesProfiles.Add(deviceToMove);
            }

        }

        public void MoveDeviceToUnnassignedList(DeviceProfile targetDevice)
        {
            if (targetDevice != null || targetDevice.DeviceID != 0)
            {
                int realEstateIdToMoveFrom = GetRealEstateByDeviceID(targetDevice.DeviceID);
                RealEstate? realEstateToMoveFrom = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateIdToMoveFrom);
                CurrentUserWithAllData.UnassignedDevicesList.Add(targetDevice);
                realEstateToMoveFrom.DevicesProfiles.Remove(targetDevice);

            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_currentUserWithAllData"></param>
        /// <param name="realEstateToAddDevice"></param>
        /// <param name="currentDevice"></param>
        /// <returns></returns>
        public void MoveDeviceFromUnassignedDevicesProfile(UserProfile _currentUserWithAllData, int realEstateToAddDevice, DeviceProfile currentDevice)
        {
            _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == realEstateToAddDevice).DevicesProfiles.Add(currentDevice);
            _currentUserWithAllData.UnassignedDevicesList.Remove(currentDevice);
        }

        /// <summary>
        /// Method to add device ID in the list of selected devices to print QR codes. This list will be used in the page to print QR codes for selected devices. If device ID is already in the list so it will not be added again.
        /// </summary>
        /// <param name="deviceId">Selected Device ID</param>
        public void AddToPrintQueue(DeviceProfile selectedDevice)
        {
            if (!SelectedDevicesListToPrintQrCodes.Any(d => d.DeviceID == selectedDevice.DeviceID))
            {
                SelectedDevicesListToPrintQrCodes.Add(selectedDevice);
            }
        }

        /// <summary>
        /// Method to get list of selected devices to print QR codes and clear the list after getting it. This method will be used in the page to print QR codes for selected devices. After getting the list of selected devices to print QR codes, the list will be cleared to avoid printing QR codes for the same devices again.
        /// </summary>
        /// <returns></returns>
        public List<DeviceProfile> GetCurrentQueue()
        {
            List<DeviceProfile> temporaryDevicesList = SelectedDevicesListToPrintQrCodes.ToList(); // Copy the list

            return temporaryDevicesList;                                // Return the copy
        }

        public void CleanCurrentQueue()
        {
            SelectedDevicesListToPrintQrCodes.Clear();
        }

        public void RemoveFromQueue(DeviceProfile device)
        {
            DeviceProfile? deviceToRemoveFromList = SelectedDevicesListToPrintQrCodes.FirstOrDefault(d => d.DeviceID == device.DeviceID);
            if (deviceToRemoveFromList != null)
            {
                SelectedDevicesListToPrintQrCodes.Remove(deviceToRemoveFromList);
            }
        }

        public async Task AddAllToPrintQueue()
        {
            List<DeviceProfile> allDevices = await GetAllUserDevicesAsync();
            foreach (var device in allDevices)
            {
                if (!SelectedDevicesListToPrintQrCodes.Any(d => d.DeviceID == device.DeviceID))
                {
                    SelectedDevicesListToPrintQrCodes.Add(device);
                }
            }
        }
        /// <summary>
        /// Getting item in the list by ID
        /// </summary>
        /// <param name="id">ID number</param>
        /// <returns>Device Profile from the list with matched device ID</returns>
        public DeviceProfile GetDeviceById(int id)
        {
            DeviceProfile currentDevice = new();
            int i = 0;
            DeviceProfile? currentDeviceTest = new DeviceProfile();
            DeviceProfile device;
            for (i = 0; i < _currentUserWithAllData.GetAllDevices().Count; i++)
            {
                device = _currentUserWithAllData.GetAllDevices()[i];
                if (device.DeviceID == id)
                {
                    currentDevice = device;
                    break;
                }
            }
            return currentDevice;
        }

        public async Task<DeviceProfile?> GetDeviceForGuestAsync(int userId, int deviceId)
        {
            // 1. Fetch User with BOTH potential device paths included
            var user = await _dbcontext.Users
                .Include(u => u.UserProfile)
                    .ThenInclude(p => p.UnassignedDevicesList) // Path A
                        .ThenInclude(d => d.DeviceWarranty)
                .Include(u => u.UserProfile)
                    .ThenInclude(p => p.RealEstates)           // Path B
                        .ThenInclude(r => r.DevicesProfiles)
                            .ThenInclude(d => d.DeviceWarranty)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserProfile != null && u.UserProfile.UserID == userId);

            if (user?.UserProfile == null) return null;

            // 2. Check the Unassigned List first
            var unassignedDevice = user.UserProfile.UnassignedDevicesList?
                .FirstOrDefault(d => d.DeviceID == deviceId);

            if (unassignedDevice != null) return unassignedDevice;

            // 3. If not there, check the Real Estate collections
            var assignedDevice = user.UserProfile.RealEstates?
                .SelectMany(r => r.DevicesProfiles)
                .FirstOrDefault(d => d.DeviceID == deviceId);

            return assignedDevice;
        }

        public async Task Navigate(DeviceProfile currentDevice, IJSRuntime jSRuntime)
        {
            var query = new Dictionary<string, string>
            {
            { $"{currentDevice.DeviceProduser}", $"{currentDevice.DeviceModelNumber}" }
        };
            string buildedUrl = Util.BuildUrlWithQueryStringUsingStringConcat(Program.Constants.BASE_API_URL, query);
            await jSRuntime.InvokeVoidAsync("open", buildedUrl, "_blank");
        }

        #endregion


        #region ShopDetails


        #endregion

        #region Warranties

        /// <summary>
        /// Method to get a device with closest expirig date to the actual date 
        /// </summary>
        /// <returns>expiringDevice device profile</returns>
        public DeviceProfile FirstexpiringDeviceWarranty()
        {
            List<DeviceProfile>? devicesList = _currentUserWithAllData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            //List<DeviceWarranty> warranties = DevicesWarranties;
            List<DeviceProfile> validWarrantiesList = new();
            DeviceProfile firstexpiringDeviceDevice = new();
            var counting = 0;
            DateTime date = DateTime.Now;

            foreach (DeviceProfile device in devicesList)
            {
                counting = date.CompareTo(device.DeviceWarranty.WarrantyEnd);
                if (counting < 0)
                {
                    validWarrantiesList.Add(device);
                }
            }
            if (validWarrantiesList.Count != 0)
            {
                var sortedList = validWarrantiesList.OrderBy(d => d.DeviceWarranty.WarrantyEnd);
                firstexpiringDeviceDevice = sortedList.FirstOrDefault();
            }

            return firstexpiringDeviceDevice;
        }

        public static TimeSpan GetTimeSpanFromYears(int years) // add days from editform 
        {
            int totalDaysInTheYear = 365;
            int yearsToDays = years * totalDaysInTheYear;
            TimeSpan interval = TimeSpan.FromDays(yearsToDays);
            string timeInterval = interval.ToString();
            int pIndex = timeInterval.IndexOf(':');
            pIndex = timeInterval.IndexOf('.', pIndex);
            if (pIndex < 0) timeInterval += "        ";

            Console.WriteLine("{0,21}{1,26}", yearsToDays, timeInterval);
            return interval;
        }
        #endregion

        #region Unassigned Devices
        /// <summary>
        /// Moves devices to a separate list where devices are not assigned to the real estate. This can be done later.
        /// </summary>
        /// <param name="currentRealEstate">The Real Estate to be Removed</param>
        public async Task LeaveDevicesUnassigned(RealEstate currentRealEstate)
        {
            RealEstate realEstateInDB = _currentUserWithAllData.RealEstates.FirstOrDefault(r => r.RealEstateID == currentRealEstate.RealEstateID);
            if (realEstateInDB == null) return;

            List<DeviceProfile> devicesToMove = realEstateInDB.DevicesProfiles.ToList();
            foreach (DeviceProfile deviceProfile in devicesToMove)
            {
                _currentUserWithAllData.UnassignedDevicesList.Add(deviceProfile);
                realEstateInDB.DevicesProfiles.Remove(deviceProfile);
            }
            await UpdateObjectInDB();
        }
        #endregion

        #region Should be moved?


        //This method is used to upload the file by loading it in the  local memory first and later adding it to the server if all requirements are ok.

        public async Task<string> CaptureFilePathFromBytes(byte[] fileBytes, string originalName, DeviceProfile currentDevice)
        {
            // Safety check: if there are no bytes, return an empty string right away
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return "";
            }

            try
            {
                string extension = Path.GetExtension(originalName);
                string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string userId = _currentUserWithAllData.UserID.ToString();
                string deviceId = currentDevice.DeviceID.ToString();

                string baseFolder;

                // 🟢 SMART PATH CHECK: Detects if you are on Dev vs Production
                if (OperatingSystem.IsWindows())
                {
                    // Local Dev: Saves inside your project directory (e.g., C:\YourProject\Files\1\7)
                    baseFolder = Path.Combine(AppContext.BaseDirectory, "Files", userId, deviceId);
                }
                else
                {
                    // Linux Production / Docker container environment
                    baseFolder = $"/app/Files/{userId}/{deviceId}";
                }

                // Standardize separators to forward slashes so it matches database strings cleanly
                baseFolder = baseFolder.Replace('\\', '/');
                string filePath = $"{baseFolder}/{newFileName}";

                // Ensure directory exists locally or on server
                string localCreationPath = OperatingSystem.IsWindows() ? baseFolder.Replace('/', '\\') : baseFolder;
                if (!Directory.Exists(localCreationPath))
                {
                    Directory.CreateDirectory(localCreationPath);
                }

                // Write the memory array bytes directly to disk asynchronously
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                // Return the clean absolute storage string to save in the DB context
                return filePath;
            }
            catch (Exception ex)
            {
                // Log your error details to your terminal context console
                Console.WriteLine($"[DataService Disk Write Error]: {ex.Message}");
                return "";
            }
        }


        /// <summary>
        /// Checking if there is a file assigned to the filepath. 
        /// If there is so it will be deleted to avoid saving multiple files in the server for the same object. 
        /// Old file always will be deleted and then added new to the server.
        /// </summary>
        /// <param name="filePath">Path to the file in the Blazor server</param>
        public void DeleteFileIfExists(string filePath)
        {
            if (!System.String.IsNullOrEmpty(filePath))
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// Checking if file exsists and if the link is active. If not so it will be changed to an empty string.
        /// </summary>
        public void CheckIfFileExist()
        {
            // var files = Directory.GetFiles(Environment.CurrentDirectory + $"\\Files\\{DataService._currentUserWithAllData.UserID}", "*.*");
            if (_currentUserWithAllData == null)
            {
                return;
            }
            List<DeviceProfile> deviceProfiles = _currentUserWithAllData.RealEstates.SelectMany(realEstate => realEstate.DevicesProfiles).ToList();
            foreach (DeviceProfile device in deviceProfiles)
            {
                if (!System.IO.File.Exists(device.DeviceWarranty.ReceiptLink))
                {
                    device.DeviceWarranty.ReceiptLink = "";
                }
                if (!System.IO.File.Exists(device.DeviceWarranty.ExtraInsuranceWarrantyLink))
                {
                    device.DeviceWarranty.ExtraInsuranceWarrantyLink = "";
                }
            }
        }

        public string GetFileUrl(string linkToTheFile, int deviceId)
        {
            var file = Path.GetFileName(linkToTheFile);
            string fileUrl = $"files/{_currentUserWithAllData.UserID}/{deviceId}/{file}";
            return fileUrl;
        }

        public async Task UpdateObjectInDB()
        {
            await _dbcontext.SaveChangesAsync();
        }

        #endregion


    }
}
