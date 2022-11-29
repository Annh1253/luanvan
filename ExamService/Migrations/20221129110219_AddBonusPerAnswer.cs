using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamService.Migrations
{
    public partial class AddBonusPerAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bonus",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "Answers");
        }
    }
}
