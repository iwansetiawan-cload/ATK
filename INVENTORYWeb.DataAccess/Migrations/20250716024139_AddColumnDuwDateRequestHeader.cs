using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDuwDateRequestHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DUEDATE",
                table: "REQUEST_ITEM_HEADER",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DUEDATE",
                table: "REQUEST_ITEM_HEADER");
        }
    }
}
