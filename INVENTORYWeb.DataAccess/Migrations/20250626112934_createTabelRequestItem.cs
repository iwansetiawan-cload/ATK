using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class createTabelRequestItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ITEMS_CATEGORY_CATEGORYID",
                table: "ITEMS");

            migrationBuilder.RenameColumn(
                name: "LASTMODIFYUSER",
                table: "MSUDC",
                newName: "LAST_MODIFY_USER");

            migrationBuilder.RenameColumn(
                name: "LASTMODIFYDATE",
                table: "MSUDC",
                newName: "LAST_MODIFY_DATE");

            migrationBuilder.RenameColumn(
                name: "ENTRYKEY",
                table: "MSUDC",
                newName: "ENTRY_KEY");

            migrationBuilder.RenameColumn(
                name: "CATEGORYID",
                table: "ITEMS",
                newName: "CATEGORY_ID");

            migrationBuilder.RenameIndex(
                name: "IX_ITEMS_CATEGORYID",
                table: "ITEMS",
                newName: "IX_ITEMS_CATEGORY_ID");

            migrationBuilder.CreateTable(
                name: "REQUEST_ITEM_HEADER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJECT_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REQUESTER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REQUEST_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TOTAL_QTY = table.Column<double>(type: "float", nullable: true),
                    STATUS_ID = table.Column<int>(type: "int", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    APPROVE_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APPROVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REJECTED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REJECTED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_ITEM_HEADER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_ITEM_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HEADER_ID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_NAME = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    CATEGORY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PIECE = table.Column<int>(type: "int", nullable: true),
                    QTY = table.Column<int>(type: "int", nullable: true),
                    STATUS_ID = table.Column<int>(type: "int", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APPROVE_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APPROVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REJECTED_BY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REJECTED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_ITEM_DETAIL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_REQUEST_ITEM_DETAIL_REQUEST_ITEM_HEADER_HEADER_ID",
                        column: x => x.HEADER_ID,
                        principalTable: "REQUEST_ITEM_HEADER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_ITEM_DETAIL_HEADER_ID",
                table: "REQUEST_ITEM_DETAIL",
                column: "HEADER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ITEMS_CATEGORY_CATEGORY_ID",
                table: "ITEMS",
                column: "CATEGORY_ID",
                principalTable: "CATEGORY",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ITEMS_CATEGORY_CATEGORY_ID",
                table: "ITEMS");

            migrationBuilder.DropTable(
                name: "REQUEST_ITEM_DETAIL");

            migrationBuilder.DropTable(
                name: "REQUEST_ITEM_HEADER");

            migrationBuilder.RenameColumn(
                name: "LAST_MODIFY_USER",
                table: "MSUDC",
                newName: "LASTMODIFYUSER");

            migrationBuilder.RenameColumn(
                name: "LAST_MODIFY_DATE",
                table: "MSUDC",
                newName: "LASTMODIFYDATE");

            migrationBuilder.RenameColumn(
                name: "ENTRY_KEY",
                table: "MSUDC",
                newName: "ENTRYKEY");

            migrationBuilder.RenameColumn(
                name: "CATEGORY_ID",
                table: "ITEMS",
                newName: "CATEGORYID");

            migrationBuilder.RenameIndex(
                name: "IX_ITEMS_CATEGORY_ID",
                table: "ITEMS",
                newName: "IX_ITEMS_CATEGORYID");

            migrationBuilder.AddForeignKey(
                name: "FK_ITEMS_CATEGORY_CATEGORYID",
                table: "ITEMS",
                column: "CATEGORYID",
                principalTable: "CATEGORY",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
