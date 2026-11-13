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
            return _currentUserWithAllData?
                .RealEstates
                .MaxBy(r => r.RealEstateID);
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


    }
}
