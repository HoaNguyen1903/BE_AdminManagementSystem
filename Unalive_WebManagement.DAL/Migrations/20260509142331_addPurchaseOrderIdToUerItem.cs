using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addPurchaseOrderIdToUerItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ShopOrderId",
                table: "UserItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "UserItems",
                type: "integer",
                nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Status",
            //     table: "SkinAndCharacterBundles",
            //     type: "text",
            //     nullable: false,
            //     defaultValue: "");

            // migrationBuilder.AddColumn<string>(
            //     name: "Status",
            //     table: "Items",
            //     type: "text",
            //     nullable: false,
            //     defaultValue: "");

            // migrationBuilder.AddColumn<string>(
            //     name: "Status",
            //     table: "GemBundles",
            //     type: "text",
            //     nullable: false,
            //     defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "UserItems");

            // migrationBuilder.DropColumn(
            //     name: "Status",
            //     table: "SkinAndCharacterBundles");

            // migrationBuilder.DropColumn(
            //     name: "Status",
            //     table: "Items");

            // migrationBuilder.DropColumn(
            //     name: "Status",
            //     table: "GemBundles");

            migrationBuilder.AlterColumn<int>(
                name: "ShopOrderId",
                table: "UserItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}