using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDemoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "495e6e84-f04c-434e-b740-b12ddde0bee4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aac89f82-88ea-4914-b5c8-2b987bfa18c1");

            migrationBuilder.AddColumn<bool>(
                name: "IsDemoUser",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3470e658-222c-4991-8e98-8c029cac94b5", "fe2dad0d-c1e6-4bc4-a33d-60d7001cf2ce", "User", "USER" },
                    { "b6c33b21-8d39-4910-ac5e-20391b02b8a2", "0b422cdc-a9ee-4c2e-ab86-02fec50f1865", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3470e658-222c-4991-8e98-8c029cac94b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6c33b21-8d39-4910-ac5e-20391b02b8a2");

            migrationBuilder.DropColumn(
                name: "IsDemoUser",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "495e6e84-f04c-434e-b740-b12ddde0bee4", "f6386b2b-d130-44e8-96d9-f3078538d709", "Admin", "ADMIN" },
                    { "aac89f82-88ea-4914-b5c8-2b987bfa18c1", "8954d51a-7b60-44d1-8b11-e74b360555f6", "User", "USER" }
                });
        }
    }
}
