using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class validationaddedtobranchesandbranchLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Universities");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValiddatedAt",
                table: "BranchLevels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValiddatedAt",
                table: "Branches",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValiddatedAt",
                table: "BranchLevels");

            migrationBuilder.DropColumn(
                name: "ValiddatedAt",
                table: "Branches");

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Universities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
