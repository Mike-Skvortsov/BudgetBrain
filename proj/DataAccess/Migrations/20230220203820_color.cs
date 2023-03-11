using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class color : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                schema: "dbo",
                table: "Cards",
                newName: "ColorId");

            migrationBuilder.CreateTable(
                name: "ColorCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ColorId",
                schema: "dbo",
                table: "Cards",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_ColorCards_ColorId",
                schema: "dbo",
                table: "Cards",
                column: "ColorId",
                principalTable: "ColorCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_ColorCards_ColorId",
                schema: "dbo",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "ColorCards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ColorId",
                schema: "dbo",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                schema: "dbo",
                table: "Cards",
                newName: "Color");
        }
    }
}
