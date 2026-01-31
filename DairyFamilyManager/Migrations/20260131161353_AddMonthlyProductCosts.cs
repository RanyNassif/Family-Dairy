using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DairyFamilyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyProductCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyProductCosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    MilkCost = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    WorkersCost = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    GasCost = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    OtherCost = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyProductCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyProductCosts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyProductCosts_ProductId",
                table: "MonthlyProductCosts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyProductCosts_Year_Month_ProductId",
                table: "MonthlyProductCosts",
                columns: new[] { "Year", "Month", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyProductCosts");
        }
    }
}
