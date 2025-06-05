using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class removedKeyFromDeviceProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17a3c51c-33fd-4f8d-bd44-b33542eca3ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e86005de-c018-4241-ad42-4c2a2963edfc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b5ce0357-5bce-4326-9d94-eb164c5027f9", "bf8336e2-cf29-47fc-a64e-39ef3d7e224b", "Admin", "ADMIN" },
                    { "e87efaa0-7e5c-4e18-be7c-2e0d43bd62bc", "4d2b90b0-27df-41b7-a575-84a6a4b10441", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5ce0357-5bce-4326-9d94-eb164c5027f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e87efaa0-7e5c-4e18-be7c-2e0d43bd62bc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17a3c51c-33fd-4f8d-bd44-b33542eca3ea", "132c5d22-6f38-4a39-af18-93d99e2a1ab3", "User", "USER" },
                    { "e86005de-c018-4241-ad42-4c2a2963edfc", "5105ce72-953c-48b8-bcd0-6601e53713cd", "Admin", "ADMIN" }
                });
        }
    }
}
