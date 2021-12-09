using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class BranchesrenamedtoFormations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_BranchLevelRegistrationRequirements_BranchLevelRegistrationRequirementId",
                table: "SubscriptionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_BranchLevels_BranchLevelId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "BranchLevelRegistrationRequirements");

            migrationBuilder.DropTable(
                name: "BranchLevels");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.RenameColumn(
                name: "BranchLevelId",
                table: "Subscriptions",
                newName: "FormationLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_BranchLevelId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_FormationLevelId");

            migrationBuilder.RenameColumn(
                name: "BranchLevelRegistrationRequirementId",
                table: "SubscriptionLines",
                newName: "FormationLevelFileId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_BranchLevelRegistrationRequirementId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_FormationLevelFileId");

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgLink2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValiddatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formations_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormationLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    ValiddatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "FormationLevelFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOrder = table.Column<int>(type: "int", nullable: false),
                    FormationLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FormationLevelFiles_FormationLevelId",
                table: "FormationLevelFiles",
                column: "FormationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FormationLevels_FormationId",
                table: "FormationLevels",
                column: "FormationId");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_UniversityId",
                table: "Formations",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_FormationLevelFiles_FormationLevelFileId",
                table: "SubscriptionLines",
                column: "FormationLevelFileId",
                principalTable: "FormationLevelFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_FormationLevels_FormationLevelId",
                table: "Subscriptions",
                column: "FormationLevelId",
                principalTable: "FormationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionLines_FormationLevelFiles_FormationLevelFileId",
                table: "SubscriptionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_FormationLevels_FormationLevelId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "FormationLevelFiles");

            migrationBuilder.DropTable(
                name: "FormationLevels");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.RenameColumn(
                name: "FormationLevelId",
                table: "Subscriptions",
                newName: "BranchLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_FormationLevelId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_BranchLevelId");

            migrationBuilder.RenameColumn(
                name: "FormationLevelFileId",
                table: "SubscriptionLines",
                newName: "BranchLevelRegistrationRequirementId");

            migrationBuilder.RenameIndex(
                name: "IX_SubscriptionLines_FormationLevelFileId",
                table: "SubscriptionLines",
                newName: "IX_SubscriptionLines_BranchLevelRegistrationRequirementId");

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgLink2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValiddatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    ValiddatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchLevels_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchLevelRegistrationRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLevelRegistrationRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchLevelRegistrationRequirements_BranchLevels_BranchLevelId",
                        column: x => x.BranchLevelId,
                        principalTable: "BranchLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UniversityId",
                table: "Branches",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLevelRegistrationRequirements_BranchLevelId",
                table: "BranchLevelRegistrationRequirements",
                column: "BranchLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLevels_BranchId",
                table: "BranchLevels",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionLines_BranchLevelRegistrationRequirements_BranchLevelRegistrationRequirementId",
                table: "SubscriptionLines",
                column: "BranchLevelRegistrationRequirementId",
                principalTable: "BranchLevelRegistrationRequirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_BranchLevels_BranchLevelId",
                table: "Subscriptions",
                column: "BranchLevelId",
                principalTable: "BranchLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
