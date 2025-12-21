using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class changeItinerary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameProduct",
                table: "Itineraries",
                newName: "AUDName");

            migrationBuilder.AddColumn<string>(
                name: "CPC",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DivisionID",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "PaymentCoefficient",
                table: "OperationsOfItinerary",
                type: "numeric(6,3)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "numeric(6,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "OperationCategories",
                type: "numeric(8,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DivisionID",
                table: "Products",
                column: "DivisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Divisions_DivisionID",
                table: "Products",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Divisions_DivisionID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DivisionID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CPC",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DivisionID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "AUDName",
                table: "Itineraries",
                newName: "NameProduct");

            migrationBuilder.AlterColumn<float>(
                name: "PaymentCoefficient",
                table: "OperationsOfItinerary",
                type: "numeric(6,4)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "numeric(6,3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "OperationCategories",
                type: "numeric(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(8,4)");
        }
    }
}
