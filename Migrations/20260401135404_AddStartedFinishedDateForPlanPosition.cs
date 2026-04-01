using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class AddStartedFinishedDateForPlanPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "FinishedDate",
                table: "PlanPositions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartedDate",
                table: "PlanPositions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedDate",
                table: "PlanPositions");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "PlanPositions");
        }
    }
}
