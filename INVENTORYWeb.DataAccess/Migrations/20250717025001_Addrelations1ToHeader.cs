using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addrelations1ToHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "S1_ID",
                table: "REQUEST_ITEM_HEADER",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S1_ID",
                table: "REQUEST_ITEM_HEADER");
        }
    }
}
