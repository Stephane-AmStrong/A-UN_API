using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class paymentrenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compteur",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FA_CompteurTotal",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FA_CompteurType",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "IFU",
                table: "Payments",
                newName: "Feda_Status");

            migrationBuilder.RenameColumn(
                name: "FV_SIG",
                table: "Payments",
                newName: "Feda_Mode");

            migrationBuilder.RenameColumn(
                name: "FV_NIM",
                table: "Payments",
                newName: "Feda_Klass");

            migrationBuilder.RenameColumn(
                name: "FV_DateMECef",
                table: "Payments",
                newName: "Feda_Id");

            migrationBuilder.RenameColumn(
                name: "FV_CompteurType",
                table: "Payments",
                newName: "Feda_Description");

            migrationBuilder.RenameColumn(
                name: "FV_CompteurTotal",
                table: "Payments",
                newName: "Feda_Customer_id");

            migrationBuilder.RenameColumn(
                name: "FA_SIG",
                table: "Payments",
                newName: "Feda_Currency_id");

            migrationBuilder.RenameColumn(
                name: "FA_NIM",
                table: "Payments",
                newName: "Feda_CallbackUrl");

            migrationBuilder.RenameColumn(
                name: "FA_DateMECef",
                table: "Payments",
                newName: "Feda_Amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feda_Status",
                table: "Payments",
                newName: "IFU");

            migrationBuilder.RenameColumn(
                name: "Feda_Mode",
                table: "Payments",
                newName: "FV_SIG");

            migrationBuilder.RenameColumn(
                name: "Feda_Klass",
                table: "Payments",
                newName: "FV_NIM");

            migrationBuilder.RenameColumn(
                name: "Feda_Id",
                table: "Payments",
                newName: "FV_DateMECef");

            migrationBuilder.RenameColumn(
                name: "Feda_Description",
                table: "Payments",
                newName: "FV_CompteurType");

            migrationBuilder.RenameColumn(
                name: "Feda_Customer_id",
                table: "Payments",
                newName: "FV_CompteurTotal");

            migrationBuilder.RenameColumn(
                name: "Feda_Currency_id",
                table: "Payments",
                newName: "FA_SIG");

            migrationBuilder.RenameColumn(
                name: "Feda_CallbackUrl",
                table: "Payments",
                newName: "FA_NIM");

            migrationBuilder.RenameColumn(
                name: "Feda_Amount",
                table: "Payments",
                newName: "FA_DateMECef");

            migrationBuilder.AddColumn<string>(
                name: "Compteur",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FA_CompteurTotal",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FA_CompteurType",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
