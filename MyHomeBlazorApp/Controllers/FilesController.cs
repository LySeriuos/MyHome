using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyHomeBlazorApp.BlazorData;


namespace MyHomeBlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly UserManager<MyHomeBlazorAppUser> _userManager;
        public FilesController(DataService dataService, UserManager<MyHomeBlazorAppUser> userManager)
        {
            _dataService = dataService;
            _userManager = userManager;
        }

        [HttpGet("{deviceId}/{fileName}")]
        public async Task<IActionResult> GetSecureFile(int deviceId, string fileName)
        {
            string identityUserId = _userManager.GetUserId(User);
            if(string.IsNullOrEmpty(identityUserId)) {
                return Unauthorized();
            }
            bool isOwner = await _dataService.IsDeviceOwnedByUserAsync(deviceId, identityUserId);
            if (!isOwner)
            {
                return Forbid();
            }

            string baseFolder = Path.Combine("/", "app", "Files", identityUserId, deviceId.ToString());
            string filePath = Path.Combine(baseFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
