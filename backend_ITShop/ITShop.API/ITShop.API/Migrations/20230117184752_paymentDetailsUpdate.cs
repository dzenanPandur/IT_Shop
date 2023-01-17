using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITShop.API.Migrations
{
    public partial class paymentDetailsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Orders_OrderID",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_OrderID",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "PaymentDetails");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders",
                column: "PaymentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "PaymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_OrderID",
                table: "PaymentDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders",
                column: "PaymentID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Orders_OrderID",
                table: "PaymentDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
