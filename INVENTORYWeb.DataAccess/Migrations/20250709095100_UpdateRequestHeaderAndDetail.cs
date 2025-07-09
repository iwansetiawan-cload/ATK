using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestHeaderAndDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "COMPLETED_BY",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "COMPLETED_DATE",
                table: "REQUEST_ITEM_HEADER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "COMPLETED_NOTES",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "COMPLETED_BY",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "COMPLETED_DATE",
                table: "REQUEST_ITEM_DETAIL",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "COMPLETED_NOTES",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QTY_ADJUST",
                table: "REQUEST_ITEM_DETAIL",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "REMARK_ADJUST",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COMPLETED_BY",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "COMPLETED_DATE",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "COMPLETED_NOTES",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "COMPLETED_BY",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "COMPLETED_DATE",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "COMPLETED_NOTES",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "QTY_ADJUST",
                table: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropColumn(
                name: "REMARK_ADJUST",
                table: "REQUEST_ITEM_DETAIL");
        }
    }
}
