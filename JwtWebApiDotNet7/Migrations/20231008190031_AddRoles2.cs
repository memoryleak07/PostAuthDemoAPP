using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiDemoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "2627962d-061e-4c00-992a-4d282543417a");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "548d27b3-de02-46e2-812e-0a1e1eeebeae");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "abf71ebe-46b9-4188-a0d0-433927d0fb44");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04d1c71f-4f39-48a8-a167-1969a9d573c2", "2", "User", "USER" },
                    { "14e0f811-d2e7-4f9b-9b77-759708feabb5", "1", "Admin", "ADMIN" },
                    { "825bbeb4-5fa7-4a32-8641-0931c5cc4fd6", "3", "Member", "MEMBER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "04d1c71f-4f39-48a8-a167-1969a9d573c2");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "14e0f811-d2e7-4f9b-9b77-759708feabb5");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "825bbeb4-5fa7-4a32-8641-0931c5cc4fd6");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2627962d-061e-4c00-992a-4d282543417a", "2", "User", "USER" },
                    { "548d27b3-de02-46e2-812e-0a1e1eeebeae", "3", "Member", "MEMBER" },
                    { "abf71ebe-46b9-4188-a0d0-433927d0fb44", "1", "Admin", "ADMIN" }
                });
        }
    }
}
