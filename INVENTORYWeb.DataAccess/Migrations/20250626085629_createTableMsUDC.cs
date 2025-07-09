using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class createTableMsUDC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PIECE",
                table: "ITEMS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MSUDC",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ENTRYKEY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TEXT1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TEXT2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TEXT3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    INUM1 = table.Column<int>(type: "int", nullable: true),
                    INUM2 = table.Column<int>(type: "int", nullable: true),
                    MNUM1 = table.Column<double>(type: "float", nullable: true),
                    MNUM2 = table.Column<double>(type: "float", nullable: true),
                    CREATOR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LASTMODIFYUSER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LASTMODIFYDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSUDC", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MSUDC");

            migrationBuilder.DropColumn(
                name: "PIECE",
                table: "ITEMS");
        }
    }
}
