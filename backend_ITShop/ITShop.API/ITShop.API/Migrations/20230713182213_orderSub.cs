using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITShop.API.Migrations
{
    public partial class orderSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSubscribed",
                table: "Orders",
                type: "bit",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSubscribed",
                table: "Orders");

        }
    }
}
