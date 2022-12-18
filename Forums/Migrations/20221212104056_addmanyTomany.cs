using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forums.Migrations
{
    public partial class addmanyTomany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forums_levelId",
                table: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_levelId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_levelId",
                table: "Forums",
                column: "levelId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_levelId",
                table: "AspNetUsers",
                column: "levelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Forums_levelId",
                table: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_levelId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_levelId",
                table: "Forums",
                column: "levelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_levelId",
                table: "AspNetUsers",
                column: "levelId",
                unique: true);
        }
    }
}
