using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiDemoApp.Migrations
{
    /// <inheritdoc />
    public partial class User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b1ceedb-4660-4cd6-b3ed-7c6f37e610df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ddd356b-7894-4ad0-ba46-9db396e6dfe9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf8af5ea-fba6-414a-b5aa-df38300c1b7c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "355c0984-e43e-4ca3-8122-fae6e639c081", "3", "Member", "MEMBER" },
                    { "750afe8f-bf76-42c9-be23-1aa008b9c2d2", "2", "User", "USER" },
                    { "d2138f0c-1a3b-4a95-b1c3-cee2256dde69", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "355c0984-e43e-4ca3-8122-fae6e639c081");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "750afe8f-bf76-42c9-be23-1aa008b9c2d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d2138f0c-1a3b-4a95-b1c3-cee2256dde69");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b1ceedb-4660-4cd6-b3ed-7c6f37e610df", "3", "Member", "MEMBER" },
                    { "6ddd356b-7894-4ad0-ba46-9db396e6dfe9", "1", "Admin", "ADMIN" },
                    { "cf8af5ea-fba6-414a-b5aa-df38300c1b7c", "2", "User", "USER" }
                });
        }
    }
}
