using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class brancheswithtwoimgLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgLink2",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgLink2",
                table: "Branches");
        }
    }
}
