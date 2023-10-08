using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiDemoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88b58d0a-0886-4d6e-bc31-01377b4a0b94", "2", "User", "USER" },
                    { "cea6a8f3-81e1-4d50-8881-2ce69aeb5bdb", "3", "Member", "MEMBER" },
                    { "e84925c0-8d0d-494f-a5bc-3b63e280f108", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88b58d0a-0886-4d6e-bc31-01377b4a0b94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cea6a8f3-81e1-4d50-8881-2ce69aeb5bdb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e84925c0-8d0d-494f-a5bc-3b63e280f108");
        }
    }
}
