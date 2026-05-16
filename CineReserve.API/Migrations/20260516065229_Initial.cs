using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineReserve.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TicketDetails_BookingId",
                table: "TicketDetails",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketDetails_Bookings_BookingId",
                table: "TicketDetails",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketDetails_Bookings_BookingId",
                table: "TicketDetails");

            migrationBuilder.DropIndex(
                name: "IX_TicketDetails_BookingId",
                table: "TicketDetails");
        }
    }
}
