using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxTravel.app.Migrations
{
    /// <inheritdoc />
    public partial class changesOnTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Agencies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Agencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Agencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Agencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Agencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
