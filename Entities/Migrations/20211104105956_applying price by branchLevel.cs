using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class applyingpricebybranchLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "BranchLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BranchLevels");
        }
    }
}
