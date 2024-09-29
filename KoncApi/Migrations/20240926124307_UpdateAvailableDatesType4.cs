using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoncApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailableDatesType4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Events_BookingId",
                table: "Events",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_UserId",
                table: "Booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_VenueId",
                table: "Booking",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Users_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Venues_VenueId",
                table: "Booking",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Booking_BookingId",
                table: "Events",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Users_UserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Venues_VenueId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Booking_BookingId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_BookingId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Booking_UserId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_VenueId",
                table: "Booking");
        }
    }
}
