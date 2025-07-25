﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovecolumnAdjustRequestItemDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QTY_ADJUST",
                table: "REQUEST_ITEM_DETAIL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QTY_ADJUST",
                table: "REQUEST_ITEM_DETAIL",
                type: "int",
                nullable: true);
        }
    }
}
