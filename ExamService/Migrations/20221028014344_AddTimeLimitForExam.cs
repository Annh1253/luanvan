using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamService.Migrations
{
    public partial class AddTimeLimitForExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Exams",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Exams");
        }
    }
}
