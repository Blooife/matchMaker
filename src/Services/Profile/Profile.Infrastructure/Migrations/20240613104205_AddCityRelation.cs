using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationUserProfile");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CityId",
                table: "Profiles",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_City_CityId",
                table: "Profiles",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_City_CityId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_CityId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Profiles");

            migrationBuilder.CreateTable(
                name: "EducationUserProfile",
                columns: table => new
                {
                    EducationsId = table.Column<int>(type: "integer", nullable: false),
                    ProfilesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationUserProfile", x => new { x.EducationsId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_EducationUserProfile_Educations_EducationsId",
                        column: x => x.EducationsId,
                        principalTable: "Educations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationUserProfile_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationUserProfile_ProfilesId",
                table: "EducationUserProfile",
                column: "ProfilesId");
        }
    }
}
