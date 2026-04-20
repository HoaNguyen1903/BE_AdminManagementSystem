using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "ShopOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AmountPaid",
                table: "ShopOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AmountRemaining",
                table: "ShopOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Bin",
                table: "ShopOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CanceledAt",
                table: "ShopOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ShopOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiredAt",
                table: "ShopOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastTransactionUpdate",
                table: "ShopOrders",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderCode",
                table: "ShopOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "OrderTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderCode = table.Column<long>(type: "bigint", nullable: false),
                    PaymentLinkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    VirtualAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VirtualAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterAccountBankId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterAccountBankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterAccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTransactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderTransactions");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "AmountRemaining",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Bin",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "CanceledAt",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "LastTransactionUpdate",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "ShopOrders");
        }
    }
}
