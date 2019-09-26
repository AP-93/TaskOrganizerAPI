using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskOrganizerAPI.Migrations
{
    public partial class addListnCardPos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardPosition",
                table: "Card",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ListPosition",
                table: "BoardList",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardPosition",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "ListPosition",
                table: "BoardList");
        }
    }
}
