using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class Tagssettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogData_Category_CategoryId",
                table: "BlogData");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "BlogData",
                newName: "CategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_BlogData_CategoryId",
                table: "BlogData",
                newName: "IX_BlogData_CategoryName");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "BlogData",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogData_Category_CategoryName",
                table: "BlogData",
                column: "CategoryName",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogData_Category_CategoryName",
                table: "BlogData");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "BlogData",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogData_CategoryName",
                table: "BlogData",
                newName: "IX_BlogData_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "BlogData",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogData_Category_CategoryId",
                table: "BlogData",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
