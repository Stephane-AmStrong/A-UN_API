using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class Categoryaddedwhileformationlevelisremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_FormationLevels_FormationLevelId",
                table: "Prerequisites");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_FormationLevels_FormationLevelId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "FormationLevels");

            migrationBuilder.RenameColumn(
                name: "ValiddatedAt",
                table: "Subscriptions",
                newName: "ValidatedAt");

            migrationBuilder.RenameColumn(
                name: "FormationLevelId",
                table: "Subscriptions",
                newName: "FormationId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_FormationLevelId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_FormationId");

            migrationBuilder.RenameColumn(
                name: "FormationLevelId",
                table: "Prerequisites",
                newName: "FormationId");

            migrationBuilder.RenameIndex(
                name: "IX_Prerequisites_FormationLevelId",
                table: "Prerequisites",
                newName: "IX_Prerequisites_FormationId");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Formations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Formations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidatedAt",
                table: "Formations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Formations_CategoryId",
                table: "Formations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formations_Categories_CategoryId",
                table: "Formations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_Formations_FormationId",
                table: "Prerequisites",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Formations_FormationId",
                table: "Subscriptions",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formations_Categories_CategoryId",
                table: "Formations");

            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_Formations_FormationId",
                table: "Prerequisites");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Formations_FormationId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Formations_CategoryId",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "ValidatedAt",
                table: "Formations");

            migrationBuilder.RenameColumn(
                name: "ValidatedAt",
                table: "Subscriptions",
                newName: "ValiddatedAt");

            migrationBuilder.RenameColumn(
                name: "FormationId",
                table: "Subscriptions",
                newName: "FormationLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_FormationId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_FormationLevelId");

            migrationBuilder.RenameColumn(
                name: "FormationId",
                table: "Prerequisites",
                newName: "FormationLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Prerequisites_FormationId",
                table: "Prerequisites",
                newName: "IX_Prerequisites_FormationLevelId");

            migrationBuilder.CreateTable(
                name: "FormationLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    ValiddatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationLevels_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormationLevels_FormationId",
                table: "FormationLevels",
                column: "FormationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_FormationLevels_FormationLevelId",
                table: "Prerequisites",
                column: "FormationLevelId",
                principalTable: "FormationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_FormationLevels_FormationLevelId",
                table: "Subscriptions",
                column: "FormationLevelId",
                principalTable: "FormationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
