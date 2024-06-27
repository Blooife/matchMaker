using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Authentication.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aghvdhad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "njnknsk");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "wyuewb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "335250d4-8cfb-4e24-930f-14b84746fa82", null, "Admin", "ADMIN" },
                    { "451d9c74-c18f-454d-b64c-b192aec43282", null, "User", "USER" },
                    { "58122fe2-c131-4ed7-b887-5af4bafdf92a", null, "Moderator", "MODERATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "RefreshToken", "RefreshTokenExpiredAt", "SecurityStamp", "UserName" },
                values: new object[,]
                {
                    { "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048", 0, "6c29412f-b495-41b4-ad69-ca8c57bc3176", "user@gmail.com", false, null, null, "AQAAAAIAAYagAAAAED3lMHWaqgmr49fe6a2jGgxauc0Wi7GP0GIY45P0JWjqMZ2hmi0lfQNVb+6p37zrqA==", null, null, "6648dfe1-800f-4577-9813-9bae2fee3491", null },
                    { "b2103436-0d48-4b80-8a35-2551a2b47e5b", 0, "c1686de7-e42a-4edf-bf2e-6c53759c95e4", "admin@gmail.com", false, null, null, "AQAAAAIAAYagAAAAENfE3GWrz6/r4ZibYM9Gq0EnPFOwmdCMhbL1eTm9wJfE3mUEYYvdLr67AMIW2Wsyaw==", null, null, "e8d83479-02b2-461e-9d84-2e07d4547bc7", null },
                    { "cf7ddbd5-3717-489a-84ab-8c4df32780a1", 0, "4255d2fd-5f9b-4310-a878-8f50cfd4dd3f", "moderator@gmail.com", false, null, null, "AQAAAAIAAYagAAAAEKVZftZWATacjd8MiUTdD55liUv4lccFY1ycz704C5gHK3P1SNG9Ss6E6AN/UjGHLw==", null, null, "27681d6a-b22b-4b3e-ab14-bc4c7cac6e32", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "451d9c74-c18f-454d-b64c-b192aec43282", "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048" },
                    { "335250d4-8cfb-4e24-930f-14b84746fa82", "b2103436-0d48-4b80-8a35-2551a2b47e5b" },
                    { "58122fe2-c131-4ed7-b887-5af4bafdf92a", "cf7ddbd5-3717-489a-84ab-8c4df32780a1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "451d9c74-c18f-454d-b64c-b192aec43282", "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "335250d4-8cfb-4e24-930f-14b84746fa82", "b2103436-0d48-4b80-8a35-2551a2b47e5b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "58122fe2-c131-4ed7-b887-5af4bafdf92a", "cf7ddbd5-3717-489a-84ab-8c4df32780a1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "335250d4-8cfb-4e24-930f-14b84746fa82");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "451d9c74-c18f-454d-b64c-b192aec43282");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58122fe2-c131-4ed7-b887-5af4bafdf92a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b2103436-0d48-4b80-8a35-2551a2b47e5b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf7ddbd5-3717-489a-84ab-8c4df32780a1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aghvdhad", null, "Admin", "ADMIN" },
                    { "njnknsk", null, "User", "USER" },
                    { "wyuewb", null, "Moderator", "MODERATOR" }
                });
        }
    }
}
