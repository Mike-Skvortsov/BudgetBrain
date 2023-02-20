using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class CardName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CardName",
                schema: "dbo",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                schema: "dbo",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                schema: "dbo",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
