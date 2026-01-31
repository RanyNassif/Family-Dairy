using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DairyFamilyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddDailySales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailySales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySales_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailySaleLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailySaleId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UnitPriceUsed = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    FactoryProfitTypeUsed = table.Column<int>(type: "int", nullable: false),
                    FactoryProfitValueUsed = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DistributorProfitTypeUsed = table.Column<int>(type: "int", nullable: false),
                    DistributorProfitValueUsed = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySaleLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySaleLines_DailySales_DailySaleId",
                        column: x => x.DailySaleId,
                        principalTable: "DailySales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailySaleLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailySaleLines_DailySaleId_ProductId",
                table: "DailySaleLines",
                columns: new[] { "DailySaleId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_DailySaleLines_ProductId",
                table: "DailySaleLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySales_ClientId",
                table: "DailySales",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySales_Date_ClientId",
                table: "DailySales",
                columns: new[] { "Date", "ClientId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailySaleLines");

            migrationBuilder.DropTable(
                name: "DailySales");
        }
    }
}
