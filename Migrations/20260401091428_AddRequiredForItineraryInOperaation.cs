using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredForItineraryInOperaation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFormed",
                table: "OperationsOfItinerary",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFormed",
                table: "OperationsOfItinerary");
        }
    }
}
