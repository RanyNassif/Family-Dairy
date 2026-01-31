using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DairyFamilyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LabelEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LabelAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    FactoryProfitType = table.Column<int>(type: "int", nullable: false),
                    FactoryProfitValue = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DistributorProfitType = table.Column<int>(type: "int", nullable: false),
                    DistributorProfitValue = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_NameEn",
                table: "Products",
                column: "NameEn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
