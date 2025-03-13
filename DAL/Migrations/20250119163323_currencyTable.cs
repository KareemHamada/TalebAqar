using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class currencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "TbProperties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbCurrencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentState = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCurrencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbProperties_CurrencyId",
                table: "TbProperties",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbProperties_TbCurrencies_CurrencyId",
                table: "TbProperties",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbProperties_TbCurrencies_CurrencyId",
                table: "TbProperties");

            migrationBuilder.DropTable(
                name: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbProperties_CurrencyId",
                table: "TbProperties");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "TbProperties");
        }
    }
}
