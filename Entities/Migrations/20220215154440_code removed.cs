using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class coderemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "FormationLevels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Formations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "FormationLevels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
