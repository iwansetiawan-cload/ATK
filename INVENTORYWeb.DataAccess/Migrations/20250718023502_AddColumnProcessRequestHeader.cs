using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnProcessRequestHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PROCESS_BY",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PROCESS_DATE",
                table: "REQUEST_ITEM_HEADER",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PROCESS_BY",
                table: "REQUEST_ITEM_HEADER");

            migrationBuilder.DropColumn(
                name: "PROCESS_DATE",
                table: "REQUEST_ITEM_HEADER");
        }
    }
}
