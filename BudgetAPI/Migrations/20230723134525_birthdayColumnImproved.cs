using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetAPI.Migrations
{
    public partial class birthdayColumnImproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "BirtdayDate",
                table: "Users",
                newName: "BirthdayDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthdayDate",
                table: "Users",
                newName: "BirtdayDate");
        }
    }
}
