using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ItineraryOperations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainSubject",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AUDCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainSubject", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TypesOperations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOperations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Executors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isBrigade = table.Column<bool>(type: "boolean", nullable: false),
                    Members = table.Column<string[]>(type: "text[]", nullable: false),
                    DivisionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Executors_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Payment = table.Column<decimal>(type: "numeric", nullable: false),
                    DivisionID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OperationCategories_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AUDCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_MainSubject_AUDCode",
                        column: x => x.AUDCode,
                        principalTable: "MainSubject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskOrders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DivisionID = table.Column<int>(type: "integer", nullable: false),
                    ExecutorID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskOrders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaskOrders_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskOrders_Executors_ExecutorID",
                        column: x => x.ExecutorID,
                        principalTable: "Executors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanPositions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanPositions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanPositions_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PositionPlanID = table.Column<int>(type: "integer", nullable: false),
                    AUDCode = table.Column<int>(type: "integer", nullable: false),
                    NameProduct = table.Column<string>(type: "text", nullable: false),
                    NumberPositions = table.Column<int>(type: "integer", nullable: false),
                    KitIncreasingKit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Itineraries_MainSubject_AUDCode",
                        column: x => x.AUDCode,
                        principalTable: "MainSubject",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itineraries_PlanPositions_PositionPlanID",
                        column: x => x.PositionPlanID,
                        principalTable: "PlanPositions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationsOfItinerary",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItineraryID = table.Column<int>(type: "integer", nullable: false),
                    DivisionID = table.Column<int>(type: "integer", nullable: false),
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
                    NormTime = table.Column<double>(type: "double precision", nullable: false),
                    TypeOperationID = table.Column<int>(type: "integer", nullable: false),
                    NumberPositions = table.Column<int>(type: "integer", nullable: false),
                    EquipmentID = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExecutorID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PaymentCoefficient = table.Column<float>(type: "numeric(6,4)", nullable: false),
                    Reward = table.Column<float>(type: "numeric(7,3)", nullable: false),
                    DateIssue = table.Column<DateOnly>(type: "date", nullable: false),
                    DateExecution = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalWithSurcharge = table.Column<float>(type: "numeric(10,3)", nullable: false),
                    RewardAmount = table.Column<float>(type: "numeric(10,3)", nullable: false),
                    TaskOrdersID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsOfItinerary", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_Executors_ExecutorID",
                        column: x => x.ExecutorID,
                        principalTable: "Executors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_Itineraries_ItineraryID",
                        column: x => x.ItineraryID,
                        principalTable: "Itineraries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_OperationCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "OperationCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_TaskOrders_TaskOrdersID",
                        column: x => x.TaskOrdersID,
                        principalTable: "TaskOrders",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OperationsOfItinerary_TypesOperations_TypeOperationID",
                        column: x => x.TypeOperationID,
                        principalTable: "TypesOperations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Executors_DivisionID",
                table: "Executors",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_AUDCode",
                table: "Itineraries",
                column: "AUDCode");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_PositionPlanID",
                table: "Itineraries",
                column: "PositionPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationCategories_DivisionID",
                table: "OperationCategories",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_CategoryID",
                table: "OperationsOfItinerary",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_DivisionID",
                table: "OperationsOfItinerary",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_EquipmentID",
                table: "OperationsOfItinerary",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_ExecutorID",
                table: "OperationsOfItinerary",
                column: "ExecutorID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_ItineraryID",
                table: "OperationsOfItinerary",
                column: "ItineraryID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_TaskOrdersID",
                table: "OperationsOfItinerary",
                column: "TaskOrdersID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsOfItinerary_TypeOperationID",
                table: "OperationsOfItinerary",
                column: "TypeOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanPositions_ProductID",
                table: "PlanPositions",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AUDCode",
                table: "Products",
                column: "AUDCode");

            migrationBuilder.CreateIndex(
                name: "IX_TaskOrders_DivisionID",
                table: "TaskOrders",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskOrders_ExecutorID",
                table: "TaskOrders",
                column: "ExecutorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationsOfItinerary");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "OperationCategories");

            migrationBuilder.DropTable(
                name: "TaskOrders");

            migrationBuilder.DropTable(
                name: "TypesOperations");

            migrationBuilder.DropTable(
                name: "PlanPositions");

            migrationBuilder.DropTable(
                name: "Executors");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "MainSubject");
        }
    }
}
