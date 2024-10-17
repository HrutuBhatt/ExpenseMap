using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Manager.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomeMonth",
                table: "IncomeSources",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeMonth",
                table: "IncomeSources");
        }
    }
}
