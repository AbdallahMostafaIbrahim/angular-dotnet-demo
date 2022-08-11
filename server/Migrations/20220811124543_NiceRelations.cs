using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class NiceRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "imageId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_imageId",
                table: "Products",
                column: "imageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Images_imageId",
                table: "Products",
                column: "imageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Images_imageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_imageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "imageId",
                table: "Products");
        }
    }
}
