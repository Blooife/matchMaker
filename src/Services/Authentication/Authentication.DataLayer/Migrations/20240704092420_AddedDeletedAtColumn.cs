using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeletedAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048",
                columns: new[] { "ConcurrencyStamp", "DeletedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ae797520-4deb-4cb3-9e1b-0f416b7dcb3d", null, "AQAAAAIAAYagAAAAEIWqe9fBpeTL2jYjDhB6AUR69oUBQevVtsyxvxGT1k226tsRmJauNZSSTvmLeY/dnA==", "e041b0e0-088c-4c3c-842b-c64514346b50" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b2103436-0d48-4b80-8a35-2551a2b47e5b",
                columns: new[] { "ConcurrencyStamp", "DeletedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a997bf8-166d-43c6-b5ec-89780ac83700", null, "AQAAAAIAAYagAAAAEIrxCQhwjmgyS8p5kJgiQSqp0PyXxDsyy6TtTnpxQttuEdFd9IKzzR8pAX+ufL8IdA==", "80ea2f16-24dd-4a11-84c4-ef84f56e68b1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf7ddbd5-3717-489a-84ab-8c4df32780a1",
                columns: new[] { "ConcurrencyStamp", "DeletedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51e1901c-f5fa-443f-8ddd-554e5a47008b", null, "AQAAAAIAAYagAAAAEFt6nHhGV+8Kl2iBhMJVbs6UizxkBQhFjIZg5x+tLsH4JfTV7HwHyhTJDr/+FzRWAg==", "2db1e343-8406-4c61-831f-a2976e1968ff" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c29412f-b495-41b4-ad69-ca8c57bc3176", "AQAAAAIAAYagAAAAED3lMHWaqgmr49fe6a2jGgxauc0Wi7GP0GIY45P0JWjqMZ2hmi0lfQNVb+6p37zrqA==", "6648dfe1-800f-4577-9813-9bae2fee3491" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b2103436-0d48-4b80-8a35-2551a2b47e5b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1686de7-e42a-4edf-bf2e-6c53759c95e4", "AQAAAAIAAYagAAAAENfE3GWrz6/r4ZibYM9Gq0EnPFOwmdCMhbL1eTm9wJfE3mUEYYvdLr67AMIW2Wsyaw==", "e8d83479-02b2-461e-9d84-2e07d4547bc7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cf7ddbd5-3717-489a-84ab-8c4df32780a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4255d2fd-5f9b-4310-a878-8f50cfd4dd3f", "AQAAAAIAAYagAAAAEKVZftZWATacjd8MiUTdD55liUv4lccFY1ycz704C5gHK3P1SNG9Ss6E6AN/UjGHLw==", "27681d6a-b22b-4b3e-ab14-bc4c7cac6e32" });
        }
    }
}
