using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DairyFamilyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddDistributorsAndExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistributorDailyExpenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    DistributorId = table.Column<long>(type: "bigint", nullable: false),
                    BenzineAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorDailyExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributorDailyExpenses_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorDailyExpenses_Date_DistributorId",
                table: "DistributorDailyExpenses",
                columns: new[] { "Date", "DistributorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DistributorDailyExpenses_DistributorId",
                table: "DistributorDailyExpenses",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_NameEn",
                table: "Distributors",
                column: "NameEn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorDailyExpenses");

            migrationBuilder.DropTable(
                name: "Distributors");
        }
    }
}
