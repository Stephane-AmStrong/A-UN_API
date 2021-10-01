using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class ImgLinkandrenameclasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_Files_FileId",
                table: "SubscriptionLines");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "SubscriptionLines",
                newName: "PersonalFileId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_FileId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_PersonalFileId");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "AspNetUsers",
                newName: "ImgLink");

            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "RegistrationForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "BranchLevels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgLink",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonalFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalFiles_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalFiles_AppUserId",
                table: "PersonalFiles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_PersonalFiles_PersonalFileId",
                table: "SubscriptionLines",
                column: "PersonalFileId",
                principalTable: "PersonalFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_PersonalFiles_PersonalFileId",
                table: "SubscriptionLines");

            migrationBuilder.DropTable(
                name: "PersonalFiles");

            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "RegistrationForms");

            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "BranchLevels");

            migrationBuilder.DropColumn(
                name: "ImgLink",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "PersonalFileId",
                table: "SubscriptionLines",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_PersonalFileId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_FileId");

            migrationBuilder.RenameColumn(
                name: "ImgLink",
                table: "AspNetUsers",
                newName: "ImgUrl");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_AppUserId",
                table: "Files",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_Files_FileId",
                table: "SubscriptionLines",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
