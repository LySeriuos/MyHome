using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyHomeBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class deletedUnassignedModelClass : Migration
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
                    { "42325ae1-d052-469c-9355-98b2ae1da43d", "d5d04c3a-7bfe-4273-b170-75845fe5fa63", "Admin", "ADMIN" },
                    { "c68a377e-cb49-4595-ac3b-b2a6220b1815", "3211d062-866f-4498-8de9-02e4d89fc00b", "User", "USER" }
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
                keyValue: "42325ae1-d052-469c-9355-98b2ae1da43d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c68a377e-cb49-4595-ac3b-b2a6220b1815");

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
