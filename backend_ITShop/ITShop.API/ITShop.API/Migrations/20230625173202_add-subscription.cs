using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITShop.API.Migrations
{
    public partial class addsubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProducerID",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isSubscribed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_asp_net_users_UserID",
                        column: x => x.UserID,
                        principalSchema: "identity",
                        principalTable: "asp_net_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserID",
                table: "Subscriptions",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products",
                column: "ProducerID",
                principalTable: "ProductProducers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ProducerID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductProducers_ProducerID",
                table: "Products",
                column: "ProducerID",
                principalTable: "ProductProducers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
