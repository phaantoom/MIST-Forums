using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forums.Migrations
{
    public partial class addReplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "UserForums",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserForums_ParentId",
                table: "UserForums",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserForums_UserForums_ParentId",
                table: "UserForums",
                column: "ParentId",
                principalTable: "UserForums",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserForums_UserForums_ParentId",
                table: "UserForums");

            migrationBuilder.DropIndex(
                name: "IX_UserForums_ParentId",
                table: "UserForums");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "UserForums");
        }
    }
}
