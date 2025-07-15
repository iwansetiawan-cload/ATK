using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removecolumnREMARK_ADJUSTToDetailRequestitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "REMARK_ADJUST",
                table: "REQUEST_ITEM_DETAIL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "REMARK_ADJUST",
                table: "REQUEST_ITEM_DETAIL",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
