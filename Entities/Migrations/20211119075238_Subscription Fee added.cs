using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class SubscriptionFeeadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_FormationLevelFiles_FormationLevelFileId",
                table: "SubscriptionLines");

            migrationBuilder.DropTable(
                name: "FormationLevelFiles");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AcademicYears");

            migrationBuilder.RenameColumn(
                name: "FormationLevelFileId",
                table: "SubscriptionLines",
                newName: "PrerequisiteId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_FormationLevelFileId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_PrerequisiteId");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Payments",
                newName: "AcademicYearId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_SubscriptionId",
                table: "Payments",
                newName: "IX_Payments_AcademicYearId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndsOn",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartsOn",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "SubscriptionFee",
                table: "AcademicYears",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOrder = table.Column<int>(type: "int", nullable: false),
                    FormationLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prerequisites_FormationLevels_FormationLevelId",
                        column: x => x.FormationLevelId,
                        principalTable: "FormationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppUserId",
                table: "Payments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_FormationLevelId",
                table: "Prerequisites",
                column: "FormationLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AcademicYears_AcademicYearId",
                table: "Payments",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_Prerequisites_PrerequisiteId",
                table: "SubscriptionLines",
                column: "PrerequisiteId",
                principalTable: "Prerequisites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AcademicYears_AcademicYearId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_AppUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_Prerequisites_PrerequisiteId",
                table: "SubscriptionLines");

            migrationBuilder.DropTable(
                name: "Prerequisites");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AppUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EndsOn",
                table: "AcademicYears");

            migrationBuilder.DropColumn(
                name: "StartsOn",
                table: "AcademicYears");

            migrationBuilder.DropColumn(
                name: "SubscriptionFee",
                table: "AcademicYears");

            migrationBuilder.RenameColumn(
                name: "PrerequisiteId",
                table: "SubscriptionLines",
                newName: "FormationLevelFileId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_PrerequisiteId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_FormationLevelFileId");

            migrationBuilder.RenameColumn(
                name: "AcademicYearId",
                table: "Payments",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_AcademicYearId",
                table: "Payments",
                newName: "IX_Payments_SubscriptionId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentTypeId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AcademicYears",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FormationLevelFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormationLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormationLevelFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormationLevelFiles_FormationLevels_FormationLevelId",
                        column: x => x.FormationLevelId,
                        principalTable: "FormationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationLevelFiles_FormationLevelId",
                table: "FormationLevelFiles",
                column: "FormationLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Subscriptions_SubscriptionId",
                table: "Payments",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_FormationLevelFiles_FormationLevelFileId",
                table: "SubscriptionLines",
                column: "FormationLevelFileId",
                principalTable: "FormationLevelFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
