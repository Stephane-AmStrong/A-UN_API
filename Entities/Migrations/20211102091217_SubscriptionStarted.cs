using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class SubscriptionStarted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Subscriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicYearId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BranchLevelId",
                table: "Subscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscribedAt",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_AcademicYearId",
                table: "Subscriptions",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_BranchLevelId",
                table: "Subscriptions",
                column: "BranchLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AcademicYears_AcademicYearId",
                table: "Subscriptions",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_BranchLevels_BranchLevelId",
                table: "Subscriptions",
                column: "BranchLevelId",
                principalTable: "BranchLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AcademicYears_AcademicYearId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_BranchLevels_BranchLevelId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_AcademicYearId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_BranchLevelId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BranchLevelId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscribedAt",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
