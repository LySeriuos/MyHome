using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using MyHome.Models;
using MyHomeBlazorApp.BlazorData;
using MyHomeBlazorApp.Services.Files;

namespace MyHomeBlazorApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly MyHomeBlazorAppContext _db;
    private readonly UserManager<MyHomeBlazorAppUser> _users;
    private readonly DataService _data;
    private readonly DeviceFileStorage _storage;

    public FilesController(MyHomeBlazorAppContext db, UserManager<MyHomeBlazorAppUser> users,
        DataService data, DeviceFileStorage storage)
    {
        _db = db; _users = users; _data = data; _storage = storage;
    }

    [HttpGet("{deviceId:int}/{fileName}")]
    public async Task<IActionResult> GetSecureFile(int deviceId, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) || fileName != DeviceFileStorage.GetFileName(fileName))
            return BadRequest();
        var userId = _users.GetUserId(User);
        if (userId is null) return Unauthorized();
        if (!await _data.IsDeviceOwnedByUserAsync(deviceId, userId)) return Forbid();
        var user = await _db.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == userId);
        if (user?.UserProfile is null) return Forbid();
        var device = await _db.Set<DeviceProfile>().AsNoTracking()
            .Include(d => d.DeviceWarranty).FirstOrDefaultAsync(d => d.DeviceID == deviceId);
        var references = new[] { device?.DeviceWarranty?.ReceiptLink,
            device?.DeviceWarranty?.ExtraInsuranceWarrantyLink };
        foreach (var reference in references)
        {
            if (string.IsNullOrWhiteSpace(reference) || DeviceFileStorage.GetFileName(reference) != fileName)
                continue;
            var path = _storage.ResolveExisting(reference, user.UserProfile.UserID, deviceId);
            if (path is null) continue;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType)) contentType = "application/octet-stream";
            return PhysicalFile(path, contentType, fileName);
        }
        return NotFound("Document is unavailable. Its reference has been preserved.");
    }
}
