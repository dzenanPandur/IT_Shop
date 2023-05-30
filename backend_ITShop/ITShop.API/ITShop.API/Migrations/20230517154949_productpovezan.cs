using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITShop.API.Migrations
{
    public partial class productpovezan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProducerID",
                table: "Products",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductProducers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProducers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProducerID",
                table: "Products",
                column: "ProducerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products",
                column: "ProducerID",
                principalTable: "ProductProducers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductProducers");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProducerID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "Products");
        }
    }
}
