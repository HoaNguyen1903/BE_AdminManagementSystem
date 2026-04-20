using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PaymentOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OrderDate",
                table: "ShopOrders",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CancelUrl",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckoutUrl",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentLinkId",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerEmail",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerUserName",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnUrl",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ShopOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GemBundleId",
                table: "ShopOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShopOrderId1",
                table: "ShopOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "GemBundles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "GemBundles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrderDetails_ShopOrderId1",
                table: "ShopOrderDetails",
                column: "ShopOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId1",
                table: "ShopOrderDetails",
                column: "ShopOrderId1",
                principalTable: "ShopOrders",
                principalColumn: "ShopOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId1",
                table: "ShopOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShopOrderDetails_ShopOrderId1",
                table: "ShopOrderDetails");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CancelUrl",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "CheckoutUrl",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "PaymentLinkId",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "PlayerEmail",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "PlayerUserName",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "ReturnUrl",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "GemBundleId",
                table: "ShopOrderDetails");

            migrationBuilder.DropColumn(
                name: "ShopOrderId1",
                table: "ShopOrderDetails");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "GemBundles");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GemBundles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "ShopOrders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
