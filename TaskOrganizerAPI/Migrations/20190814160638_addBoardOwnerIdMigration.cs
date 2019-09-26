using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskOrganizerAPI.Migrations
{
    public partial class addBoardOwnerIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Boards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Boards");
        }
    }
}
