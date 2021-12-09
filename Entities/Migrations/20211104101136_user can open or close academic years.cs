using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class usercanopenorcloseacademicyears : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "AcademicYears",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "AcademicYears");
        }
    }
}
