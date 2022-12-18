using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forums.Migrations
{
    public partial class updateUserForum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserForums_forumId",
                table: "UserForums");

            migrationBuilder.DropColumn(
                name: "UserForumId",
                table: "Forums");

            migrationBuilder.CreateIndex(
                name: "IX_UserForums_forumId",
                table: "UserForums",
                column: "forumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserForums_forumId",
                table: "UserForums");

            migrationBuilder.AddColumn<int>(
                name: "UserForumId",
                table: "Forums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserForums_forumId",
                table: "UserForums",
                column: "forumId",
                unique: true);
        }
    }
}
