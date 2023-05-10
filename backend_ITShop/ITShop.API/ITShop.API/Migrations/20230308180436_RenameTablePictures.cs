using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITShop.API.Migrations
{
    public partial class RenameTablePictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPicture_Products_ProductID",
                table: "ProductPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPicture",
                table: "ProductPicture");

            migrationBuilder.RenameTable(
                name: "ProductPicture",
                newName: "ProductPictures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPicture_ProductID",
                table: "ProductPictures",
                newName: "IX_ProductPictures_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPictures",
                table: "ProductPictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPictures_Products_ProductID",
                table: "ProductPictures",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPictures_Products_ProductID",
                table: "ProductPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPictures",
                table: "ProductPictures");

            migrationBuilder.RenameTable(
                name: "ProductPictures",
                newName: "ProductPicture");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPictures_ProductID",
                table: "ProductPicture",
                newName: "IX_ProductPicture_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPicture",
                table: "ProductPicture",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPicture_Products_ProductID",
                table: "ProductPicture",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
