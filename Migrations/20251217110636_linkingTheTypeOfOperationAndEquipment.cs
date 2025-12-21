using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class linkingTheTypeOfOperationAndEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "OperationCategories",
                type: "numeric(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Equipment",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "TypeOperationID",
                table: "Equipment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_TypeOperationID",
                table: "Equipment",
                column: "TypeOperationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_TypesOperations_TypeOperationID",
                table: "Equipment",
                column: "TypeOperationID",
                principalTable: "TypesOperations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_TypesOperations_TypeOperationID",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_TypeOperationID",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "TypeOperationID",
                table: "Equipment");

            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "OperationCategories",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Equipment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
