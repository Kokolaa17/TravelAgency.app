using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxTravel.app.Migrations
{
    /// <inheritdoc />
    public partial class updateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationDate",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);
        }
    }
}
