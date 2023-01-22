using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemoveRemainMoney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainMoney",
                schema: "dbo",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemainMoney",
                schema: "dbo",
                table: "Cards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
