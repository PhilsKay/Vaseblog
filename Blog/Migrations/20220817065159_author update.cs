using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class authorupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComment_AspNetUsers_AuthorId",
                table: "MainComment");

            migrationBuilder.DropForeignKey(
                name: "FK_subComments_AspNetUsers_AuthorId",
                table: "subComments");

            migrationBuilder.DropIndex(
                name: "IX_subComments_AuthorId",
                table: "subComments");

            migrationBuilder.DropIndex(
                name: "IX_MainComment_AuthorId",
                table: "MainComment");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "subComments",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "MainComment",
                newName: "Author");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Author",
                table: "subComments",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "MainComment",
                newName: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_subComments_AuthorId",
                table: "subComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MainComment_AuthorId",
                table: "MainComment",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainComment_AspNetUsers_AuthorId",
                table: "MainComment",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_subComments_AspNetUsers_AuthorId",
                table: "subComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
