using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TYPE",
                table: "ITEMS");

            migrationBuilder.AddColumn<int>(
                name: "QTY",
                table: "ITEMS",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "STOCK",
                table: "ITEMS",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QTY",
                table: "ITEMS");

            migrationBuilder.DropColumn(
                name: "STOCK",
                table: "ITEMS");

            migrationBuilder.AddColumn<string>(
                name: "TYPE",
                table: "ITEMS",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
