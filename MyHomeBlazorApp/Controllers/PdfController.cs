using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHome;
using MyHome.Models;
using MyHomeBlazorApp.BlazorData;
using QuestPDF.Fluent;

namespace MyHomeBlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class PdfController : ControllerBase
    {
        private readonly DataService _dataService;
        public PdfController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("generate-qr")] // This makes the URL: /api/pdf/generate-qr
        public IActionResult GenerateQrPdf()
        {
            // 1. Check if the user is logged in and get their profile          

            var userProfile = _dataService.CurrentAppUser?.UserProfile;

            if (userProfile == null)
            {
                // Return a 401 Unauthorized or a simple message
                return Unauthorized("You must be logged in to generate QR codes.");
            }

            int currentUserId = userProfile.UserID;

            // 2. Get the devices
            // It was changed to GetCurrentQueue() just to get list of the chosed Devices. Switched from pdf to html printing.
            // Original method here was _dataService.GetQueueAndClear().
            List<DeviceProfile> devicesList = _dataService.GetCurrentQueue();

            if (devicesList.Count != 0)
            {
                return Content("No devices selected.");
            }


            // 2. Build the PDF in memory
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        // 1. Define 3 equal-width columns
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        

                        // 2. Loop through your IDs
                        foreach (var device in devicesList)
                        {
                            // Every .Cell() call automatically moves to the next spot in the 3-col grid
                            table.Cell().Padding(5).Column(col =>
                            {
                                // Future QR Image logic goes here
                                byte[] qrBytes = Logic.GetQrCodeBytes(device.DeviceID, currentUserId);
                                col.Item().Image(qrBytes);

                                col.Item().AlignCenter().Text($"Device ID: {device.DeviceID}")
                                    .FontSize(10)
                                    .SemiBold();
                                col.Item().AlignCenter().Text($"{device.DeviceName}")
                                    .FontSize(10)
                                    .SemiBold();
                            });
                        }
                    });
                });
            });

            // 3. Convert to bytes and stream
            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf");
        }
    }
}
