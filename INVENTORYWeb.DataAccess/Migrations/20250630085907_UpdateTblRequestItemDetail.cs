using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblRequestItemDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CATEGORY",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ITEM_ID",
                table: "REQUEST_ITEM_DETAIL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_ITEM_DETAIL_ITEM_ID",
                table: "REQUEST_ITEM_DETAIL",
                column: "ITEM_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_ITEM_DETAIL_ITEMS_ITEM_ID",
                table: "REQUEST_ITEM_DETAIL",
                column: "ITEM_ID",
                principalTable: "ITEMS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_ITEM_DETAIL_ITEMS_ITEM_ID",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropIndex(
                name: "IX_REQUEST_ITEM_DETAIL_ITEM_ID",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "ITEM_ID",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.AlterColumn<string>(
                name: "CATEGORY",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
