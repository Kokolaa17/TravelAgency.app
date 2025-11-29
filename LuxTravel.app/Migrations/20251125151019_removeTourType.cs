using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxTravel.app.Migrations
{
    /// <inheritdoc />
    public partial class removeTourType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours");

            migrationBuilder.DropTable(
                name: "TourTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tours_TourTypeId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TourTypeId",
                table: "Tours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourTypeId",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TourTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourTypeId",
                table: "Tours",
                column: "TourTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours",
                column: "TourTypeId",
                principalTable: "TourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
