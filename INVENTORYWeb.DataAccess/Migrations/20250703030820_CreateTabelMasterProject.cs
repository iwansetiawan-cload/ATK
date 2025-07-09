using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateTabelMasterProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QTY",
                table: "ITEMS");

            migrationBuilder.DropColumn(
                name: "STOCK",
                table: "ITEMS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
