using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class addlike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    UserLike = table.Column<bool>(type: "boolean", nullable: false),
                    DateLiked = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BlogDataBlogId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLike_BlogData_BlogDataBlogId",
                        column: x => x.BlogDataBlogId,
                        principalTable: "BlogData",
                        principalColumn: "BlogId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_BlogDataBlogId",
                table: "PostLike",
                column: "BlogDataBlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostLike");
        }
    }
}
