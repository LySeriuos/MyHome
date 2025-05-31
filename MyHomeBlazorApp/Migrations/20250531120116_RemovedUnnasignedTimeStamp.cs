using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnnasignedTimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceProfile_Unassigned_UnassignedId",
                table: "DeviceProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Unassigned_UnassignedDevicesId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "Unassigned");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_UnassignedDevicesId",
                table: "UserProfile");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63fabfa1-3fff-475c-86cb-662bf09c77cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0264440-9182-4196-8e45-fe4895e981c4");

            migrationBuilder.DropColumn(
                name: "UnassignedDevicesId",
                table: "UserProfile");

            migrationBuilder.RenameColumn(
                name: "UnassignedId",
                table: "DeviceProfile",
                newName: "UserProfileUserID");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceProfile_UnassignedId",
                table: "DeviceProfile",
                newName: "IX_DeviceProfile_UserProfileUserID");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20f73dfc-2bed-4fb9-89a9-5cfeae4ec5d3", "e6bcac14-29f1-47f3-8e45-a433457db8eb", "User", "USER" },
                    { "f3bdefe3-d063-4ebf-92e7-e6844a8e45de", "12207cae-a6ad-4d95-bc65-59b205ceef05", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceProfile_UserProfile_UserProfileUserID",
                table: "DeviceProfile",
                column: "UserProfileUserID",
                principalTable: "UserProfile",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceProfile_UserProfile_UserProfileUserID",
                table: "DeviceProfile");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20f73dfc-2bed-4fb9-89a9-5cfeae4ec5d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3bdefe3-d063-4ebf-92e7-e6844a8e45de");

            migrationBuilder.RenameColumn(
                name: "UserProfileUserID",
                table: "DeviceProfile",
                newName: "UnassignedId");

            migrationBuilder.RenameIndex(
                name: "IX_DeviceProfile_UserProfileUserID",
                table: "DeviceProfile",
                newName: "IX_DeviceProfile_UnassignedId");

            migrationBuilder.AddColumn<int>(
                name: "UnassignedDevicesId",
                table: "UserProfile",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Unassigned",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unassigned", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "63fabfa1-3fff-475c-86cb-662bf09c77cd", "91ec19c0-e34a-4ddf-9850-3cc64a19a3ef", "User", "USER" },
                    { "b0264440-9182-4196-8e45-fe4895e981c4", "44f78c35-8f87-4fbf-a05b-98531844a0ac", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UnassignedDevicesId",
                table: "UserProfile",
                column: "UnassignedDevicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceProfile_Unassigned_UnassignedId",
                table: "DeviceProfile",
                column: "UnassignedId",
                principalTable: "Unassigned",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Unassigned_UnassignedDevicesId",
                table: "UserProfile",
                column: "UnassignedDevicesId",
                principalTable: "Unassigned",
                principalColumn: "Id");
        }
    }
}
