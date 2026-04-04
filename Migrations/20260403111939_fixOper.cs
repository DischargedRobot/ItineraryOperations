using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class fixOper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Equipment_EquipmentID",
                table: "OperationsOfItinerary");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Executors_ExecutorID",
                table: "OperationsOfItinerary");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutorID",
                table: "OperationsOfItinerary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentID",
                table: "OperationsOfItinerary",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Equipment_EquipmentID",
                table: "OperationsOfItinerary",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Executors_ExecutorID",
                table: "OperationsOfItinerary",
                column: "ExecutorID",
                principalTable: "Executors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Equipment_EquipmentID",
                table: "OperationsOfItinerary");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationsOfItinerary_Executors_ExecutorID",
                table: "OperationsOfItinerary");

            migrationBuilder.AlterColumn<int>(
                name: "ExecutorID",
                table: "OperationsOfItinerary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentID",
                table: "OperationsOfItinerary",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Equipment_EquipmentID",
                table: "OperationsOfItinerary",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsOfItinerary_Executors_ExecutorID",
                table: "OperationsOfItinerary",
                column: "ExecutorID",
                principalTable: "Executors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
