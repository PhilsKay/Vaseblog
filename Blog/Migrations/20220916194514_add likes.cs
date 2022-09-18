using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class addlikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLike_BlogData_BlogDataBlogId",
                table: "PostLike");

            migrationBuilder.DropIndex(
                name: "IX_PostLike_BlogDataBlogId",
                table: "PostLike");

            migrationBuilder.DropColumn(
                name: "BlogDataBlogId",
                table: "PostLike");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_BlogId",
                table: "PostLike",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLike_BlogData_BlogId",
                table: "PostLike",
                column: "BlogId",
                principalTable: "BlogData",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLike_BlogData_BlogId",
                table: "PostLike");

            migrationBuilder.DropIndex(
                name: "IX_PostLike_BlogId",
                table: "PostLike");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogDataBlogId",
                table: "PostLike",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_BlogDataBlogId",
                table: "PostLike",
                column: "BlogDataBlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLike_BlogData_BlogDataBlogId",
                table: "PostLike",
                column: "BlogDataBlogId",
                principalTable: "BlogData",
                principalColumn: "BlogId");
        }
    }
}
