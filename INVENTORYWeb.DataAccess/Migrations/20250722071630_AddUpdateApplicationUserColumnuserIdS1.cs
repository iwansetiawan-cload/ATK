using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INVENTORYWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateApplicationUserColumnuserIdS1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserS1",
                table: "AspNetUsers",
                newName: "UserNameS1");

            migrationBuilder.AddColumn<string>(
                name: "UserIdS1",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdS1",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserNameS1",
                table: "AspNetUsers",
                newName: "UserS1");
        }
    }
}
