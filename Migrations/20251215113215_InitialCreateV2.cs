using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_MainSubject_AUDCode",
                table: "Itineraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_MainSubject_AUDCode",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "AUDCode",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AUDCode",
                table: "MainSubject",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AUDCode",
                table: "Itineraries",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MainSubject_AUDCode",
                table: "MainSubject",
                column: "AUDCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_MainSubject_AUDCode",
                table: "Itineraries",
                column: "AUDCode",
                principalTable: "MainSubject",
                principalColumn: "AUDCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MainSubject_AUDCode",
                table: "Products",
                column: "AUDCode",
                principalTable: "MainSubject",
                principalColumn: "AUDCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_MainSubject_AUDCode",
                table: "Itineraries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_MainSubject_AUDCode",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MainSubject_AUDCode",
                table: "MainSubject");

            migrationBuilder.AlterColumn<int>(
                name: "AUDCode",
                table: "Products",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "AUDCode",
                table: "MainSubject",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "AUDCode",
                table: "Itineraries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_MainSubject_AUDCode",
                table: "Itineraries",
                column: "AUDCode",
                principalTable: "MainSubject",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MainSubject_AUDCode",
                table: "Products",
                column: "AUDCode",
                principalTable: "MainSubject",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
