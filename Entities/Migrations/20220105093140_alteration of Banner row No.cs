using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class alterationofBannerrowNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    No = table.Column<int>(type: "int", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_No",
                table: "Banners",
                column: "No",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
