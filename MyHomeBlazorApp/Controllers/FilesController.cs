using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyHomeBlazorApp.BlazorData;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
namespace MyHomeBlazorApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly UserManager<MyHomeBlazorAppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public FilesController(DataService dataService, UserManager<MyHomeBlazorAppUser> userManager, IWebHostEnvironment env)
        {
            _dataService = dataService;
            _userManager = userManager;
            _env = env;
        }

        // We mapped the working route directly to your actual logic!
        [HttpGet("{deviceId}/{fileName}")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetSecureFile(int deviceId, string fileName)
        {
            // 1. Fetch the user object directly from Identity UserManager
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null) return Unauthorized();
            // 2. Grab the flat integer ID property directly from the logged-in user object
            var userWithProfile = await _dataService.DbContext.Users
         .Include(u => u.UserProfile)
         .FirstOrDefaultAsync(u => u.Id == appUser.Id);

            // 3. Security Check (Ownership verification using the string Identity ID)
            bool isOwner = await _dataService.IsDeviceOwnedByUserAsync(deviceId, appUser.Id);
            if (!isOwner) return Forbid();
            string folderUserId = userWithProfile.UserProfile.UserID.ToString();
            // 4. Align paths explicitly to your local bin target
            string baseFolder = Path.Combine(AppContext.BaseDirectory, "Files", folderUserId, deviceId.ToString());
            string filePath = Path.Combine(baseFolder, fileName);

            // 5. File Validation
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found. Server checked: {filePath}");
            }

            // 6. Content Type Detection
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType, fileName);
        }
    }
}