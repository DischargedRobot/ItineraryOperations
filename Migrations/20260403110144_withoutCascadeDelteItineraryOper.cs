using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class withoutCascadeDelteItineraryOper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Itineraries_ItineraryID",
                table: "OperationsOfItinerary");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Itineraries_ItineraryID",
                table: "OperationsOfItinerary",
                column: "ItineraryID",
                principalTable: "Itineraries",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Itineraries_ItineraryID",
                table: "OperationsOfItinerary");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Itineraries_ItineraryID",
                table: "OperationsOfItinerary",
                column: "ItineraryID",
                principalTable: "Itineraries",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
