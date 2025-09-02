using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class removedTableItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "495e6e84-f04c-434e-b740-b12ddde0bee4", "f6386b2b-d130-44e8-96d9-f3078538d709", "Admin", "ADMIN" },
                    { "aac89f82-88ea-4914-b5c8-2b987bfa18c1", "8954d51a-7b60-44d1-8b11-e74b360555f6", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "495e6e84-f04c-434e-b740-b12ddde0bee4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aac89f82-88ea-4914-b5c8-2b987bfa18c1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b5ce0357-5bce-4326-9d94-eb164c5027f9", "bf8336e2-cf29-47fc-a64e-39ef3d7e224b", "Admin", "ADMIN" },
                    { "e87efaa0-7e5c-4e18-be7c-2e0d43bd62bc", "4d2b90b0-27df-41b7-a575-84a6a4b10441", "User", "USER" }
                });
        }
    }
}
