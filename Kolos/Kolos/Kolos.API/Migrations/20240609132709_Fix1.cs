using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kolos.API.Migrations
{
    /// <inheritdoc />
    public partial class Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Payments_PaymentIdPayment",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Sales_SaleIdSale",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_PaymentIdPayment",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SaleIdSale",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PaymentIdPayment",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SaleIdSale",
                table: "Subscriptions");

            migrationBuilder.CreateTable(
                name: "PaymentSubscription",
                columns: table => new
                {
                    PaymentsIdPayment = table.Column<int>(type: "int", nullable: false),
                    SubscriptionsIdSubscription = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSubscription", x => new { x.PaymentsIdPayment, x.SubscriptionsIdSubscription });
                    table.ForeignKey(
                        name: "FK_PaymentSubscription_Payments_PaymentsIdPayment",
                        column: x => x.PaymentsIdPayment,
                        principalTable: "Payments",
                        principalColumn: "IdPayment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentSubscription_Subscriptions_SubscriptionsIdSubscription",
                        column: x => x.SubscriptionsIdSubscription,
                        principalTable: "Subscriptions",
                        principalColumn: "IdSubscription",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_IdSubscription",
                table: "Sales",
                column: "IdSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSubscription_SubscriptionsIdSubscription",
                table: "PaymentSubscription",
                column: "SubscriptionsIdSubscription");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Subscriptions_IdSubscription",
                table: "Sales",
                column: "IdSubscription",
                principalTable: "Subscriptions",
                principalColumn: "IdSubscription",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Subscriptions_IdSubscription",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "PaymentSubscription");

            migrationBuilder.DropIndex(
                name: "IX_Sales_IdSubscription",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "PaymentIdPayment",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleIdSale",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PaymentIdPayment",
                table: "Subscriptions",
                column: "PaymentIdPayment");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SaleIdSale",
                table: "Subscriptions",
                column: "SaleIdSale");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Payments_PaymentIdPayment",
                table: "Subscriptions",
                column: "PaymentIdPayment",
                principalTable: "Payments",
                principalColumn: "IdPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Sales_SaleIdSale",
                table: "Subscriptions",
                column: "SaleIdSale",
                principalTable: "Sales",
                principalColumn: "IdSale");
        }
    }
}
