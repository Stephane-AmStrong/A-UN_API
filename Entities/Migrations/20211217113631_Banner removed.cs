using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class Bannerremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropColumn(
                name: "ValiddatedAt",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "ValiddatedAt",
                table: "Formations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ValiddatedAt",
                table: "Universities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValiddatedAt",
                table: "Formations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });
        }
    }
}
