using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63fabfa1-3fff-475c-86cb-662bf09c77cd", "91ec19c0-e34a-4ddf-9850-3cc64a19a3ef", "User", "USER" },
                    { "b0264440-9182-4196-8e45-fe4895e981c4", "44f78c35-8f87-4fbf-a05b-98531844a0ac", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63fabfa1-3fff-475c-86cb-662bf09c77cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0264440-9182-4196-8e45-fe4895e981c4");
        }
    }
}
