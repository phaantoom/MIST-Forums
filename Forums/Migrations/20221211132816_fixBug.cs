using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forums.Migrations
{
    public partial class fixBug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserForums_userId",
                table: "UserForums");

            migrationBuilder.DropColumn(
                name: "userForumId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_UserForums_userId",
                table: "UserForums",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserForums_userId",
                table: "UserForums");

            migrationBuilder.AddColumn<int>(
                name: "userForumId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserForums_userId",
                table: "UserForums",
                column: "userId",
                unique: true);
        }
    }
}
