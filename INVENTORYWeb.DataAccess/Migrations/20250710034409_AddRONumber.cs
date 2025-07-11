using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRONumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "REQUEST_ORDER_NO",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ROW_NUMBER",
                table: "REQUEST_ITEM_HEADER",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ROW_NUMBER",
                table: "REQUEST_ITEM_DETAIL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "REQUEST_ORDER_NO",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "ROW_NUMBER",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "ROW_NUMBER",
                table: "REQUEST_ITEM_DETAIL");
        }
    }
}
