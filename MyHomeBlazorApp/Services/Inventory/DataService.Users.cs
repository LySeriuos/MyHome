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
                expiringDeviceDevices = WarrantyQueries.ExpiringDevicesWarrantiesInDays(_currentUserWithAllData, 180);
                FirstExpiringDevice = FirstExpiringDeviceWarranty();
                DevicesWarranties = WarrantyQueries.GetUserDevicesWarranties(_currentUserWithAllData);
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

    }
}
