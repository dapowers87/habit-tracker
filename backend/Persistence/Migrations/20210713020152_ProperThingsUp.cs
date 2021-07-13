using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ProperThingsUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Windows");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Windows",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Windows_UserId",
                table: "Windows",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Windows_Users_UserId",
                table: "Windows",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Windows_Users_UserId",
                table: "Windows");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Windows_UserId",
                table: "Windows");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Windows");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Windows",
                type: "TEXT",
                nullable: true);
        }
    }
}
