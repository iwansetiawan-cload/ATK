using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelTransaksi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "REJECTED_NOTES",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "REJECTED_NOTES",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "STOCK",
                table: "REQUEST_ITEM_DETAIL",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "REJECTED_NOTES",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "REJECTED_NOTES",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "STOCK",
                table: "REQUEST_ITEM_DETAIL");
        }
    }
}
