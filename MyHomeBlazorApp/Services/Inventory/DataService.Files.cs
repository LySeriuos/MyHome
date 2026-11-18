using MyHome.Models;
using MyHomeBlazorApp.Services.Files;

namespace MyHomeBlazorApp.BlazorData;

public partial class DataService
{
    public async Task<string> CaptureFilePathFromBytes(byte[] fileBytes, string originalName, DeviceProfile currentDevice)
    {
        if (fileBytes is null || fileBytes.Length == 0) return "";
        await EnsureUserDataLoadedAsync();
        if (!await IsDeviceOwnedByUserAsync(currentDevice.DeviceID, CurrentAppUser!.Id))
            throw new UnauthorizedAccessException("Device access denied.");
        try
        {
            return await _fileStorage.SaveAsync(_currentUserWithAllData!.UserID,
                currentDevice.DeviceID, fileBytes, originalName);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File upload failed: {ex.Message}");
            return "";
        }
    }

    public string? ResolveDeviceFilePath(string reference, int deviceId)
    {
        if (_currentUserWithAllData is null || !_currentUserWithAllData.GetAllDevices()
            .Any(d => d.DeviceID == deviceId)) return null;
        return _fileStorage.ResolveExisting(reference, _currentUserWithAllData.UserID, deviceId);
    }

    public void DeleteFileIfExists(string filePath)
    {
        // Old references are retained in callers until after replacement; find the current user's
        // device from the key. Legacy user-level paths are resolved only under that user's root.
        if (_currentUserWithAllData is null || string.IsNullOrWhiteSpace(filePath)) return;
        foreach (var device in _currentUserWithAllData.GetAllDevices())
        {
            var existing = _fileStorage.ResolveExisting(filePath, _currentUserWithAllData.UserID, device.DeviceID);
            if (existing is not null) { File.Delete(existing); return; }
        }
    }

    // Kept for existing callers. Missing local files must never erase database references.
    public void CheckIfFileExist() { }

    public string GetFileUrl(string linkToTheFile, int deviceId) =>
        $"/api/Files/{deviceId}/{Uri.EscapeDataString(DeviceFileStorage.GetFileName(linkToTheFile))}";
}
