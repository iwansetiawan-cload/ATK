using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateREQUEST_ITEM_HEADER_Table_REQUEST_DATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "REQUESTDATE",
                table: "REQUEST_ITEM_HEADER",
                newName: "REQUEST_DATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "REQUEST_DATE",
                table: "REQUEST_ITEM_HEADER",
                newName: "REQUESTDATE");
        }
    }
}
