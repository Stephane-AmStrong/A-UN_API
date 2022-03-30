using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class FormationUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgLink2",
                table: "Formations");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Formations");

            migrationBuilder.AddColumn<string>(
                name: "ImgLink2",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
