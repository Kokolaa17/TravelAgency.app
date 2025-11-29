using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxTravel.app.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Users_OwnerId",
                table: "Agencies");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyReviews_Agencies_AgencyId",
                table: "AgencyReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tours_TourId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Agencies_AgencyId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Tours_TourId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_AgencyReviews_UserId",
                table: "AgencyReviews");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_OwnerId",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "GuideId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "MinParticipants",
                table: "Tours");

            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId_TourId",
                table: "Wishlists",
                columns: new[] { "UserId", "TourId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_TourId",
                table: "Reviews",
                columns: new[] { "UserId", "TourId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgencyReviews_UserId_AgencyId",
                table: "AgencyReviews",
                columns: new[] { "UserId", "AgencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_OwnerId",
                table: "Agencies",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Users_OwnerId",
                table: "Agencies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyReviews_Agencies_AgencyId",
                table: "AgencyReviews",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tours_TourId",
                table: "Reviews",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Agencies_AgencyId",
                table: "Tours",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours",
                column: "TourTypeId",
                principalTable: "TourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Tours_TourId",
                table: "Wishlists",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Users_OwnerId",
                table: "Agencies");

            migrationBuilder.DropForeignKey(
                name: "FK_AgencyReviews_Agencies_AgencyId",
                table: "AgencyReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Tours_TourId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Agencies_AgencyId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Tours_TourId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_UserId_TourId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_TourId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_AgencyReviews_UserId_AgencyId",
                table: "AgencyReviews");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_OwnerId",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "AgencyId",
                table: "Tours",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GuideId",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "Tours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MinParticipants",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyReviews_UserId",
                table: "AgencyReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_OwnerId",
                table: "Agencies",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Users_OwnerId",
                table: "Agencies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyReviews_Agencies_AgencyId",
                table: "AgencyReviews",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Tours_TourId",
                table: "Bookings",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Tours_TourId",
                table: "Reviews",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Agencies_AgencyId",
                table: "Tours",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_TourTypes_TourTypeId",
                table: "Tours",
                column: "TourTypeId",
                principalTable: "TourTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Tours_TourId",
                table: "Wishlists",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
