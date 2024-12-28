using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyContractImage",
                table: "TbOwners");

            migrationBuilder.AddColumn<int>(
                name: "PostDays",
                table: "TbProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PropertyContractImage",
                table: "TbProperties",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostDays",
                table: "TbProperties");

            migrationBuilder.DropColumn(
                name: "PropertyContractImage",
                table: "TbProperties");

            migrationBuilder.AddColumn<string>(
                name: "PropertyContractImage",
                table: "TbOwners",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
