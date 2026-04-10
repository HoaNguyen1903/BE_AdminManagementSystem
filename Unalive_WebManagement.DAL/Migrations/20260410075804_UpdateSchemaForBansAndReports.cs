using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForBansAndReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BundleItems_ItemBundles_BundleId",
                table: "BundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrderDetails_ItemBundles_ItemBundleId",
                table: "ShopOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId",
                table: "ShopOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBundles_ItemBundles_ItemBundleId",
                table: "UserBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_ShopOrders_ShopOrderId",
                table: "UserItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_Users_UserId",
                table: "UserItems");

            migrationBuilder.DropTable(
                name: "ItemBundles");

            migrationBuilder.RenameColumn(
                name: "ItemBundleId",
                table: "UserBundles",
                newName: "SkinAndCharacterBundleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBundles_ItemBundleId",
                table: "UserBundles",
                newName: "IX_UserBundles_SkinAndCharacterBundleId");

            migrationBuilder.RenameColumn(
                name: "ItemBundleId",
                table: "ShopOrderDetails",
                newName: "SkinAndCharacterBundleId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopOrderDetails_ItemBundleId",
                table: "ShopOrderDetails",
                newName: "IX_ShopOrderDetails_SkinAndCharacterBundleId");

            migrationBuilder.RenameColumn(
                name: "BundleId",
                table: "BundleItems",
                newName: "SkinAndCharacterBundleId");

            migrationBuilder.CreateTable(
                name: "SkinAndCharacterBundles",
                columns: table => new
                {
                    SkinAndCharacterBundleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BundleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BundlePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinAndCharacterBundles", x => x.SkinAndCharacterBundleId);
                });

            migrationBuilder.CreateTable(
                name: "UserBanLogs",
                columns: table => new
                {
                    UserBanLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BanReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BannedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BannedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBanLogs", x => x.UserBanLogId);
                    table.ForeignKey(
                        name: "FK_UserBanLogs_Staffs_BannedBy",
                        column: x => x.BannedBy,
                        principalTable: "Staffs",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBanLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBanLogs_BannedBy",
                table: "UserBanLogs",
                column: "BannedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserBanLogs_UserId",
                table: "UserBanLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BundleItems_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "BundleItems",
                column: "SkinAndCharacterBundleId",
                principalTable: "SkinAndCharacterBundles",
                principalColumn: "SkinAndCharacterBundleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId",
                table: "ShopOrderDetails",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "ShopOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrderDetails_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "ShopOrderDetails",
                column: "SkinAndCharacterBundleId",
                principalTable: "SkinAndCharacterBundles",
                principalColumn: "SkinAndCharacterBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBundles_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "UserBundles",
                column: "SkinAndCharacterBundleId",
                principalTable: "SkinAndCharacterBundles",
                principalColumn: "SkinAndCharacterBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_ShopOrders_ShopOrderId",
                table: "UserItems",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "ShopOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_Users_UserId",
                table: "UserItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BundleItems_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "BundleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId",
                table: "ShopOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrderDetails_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "ShopOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBundles_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                table: "UserBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_ShopOrders_ShopOrderId",
                table: "UserItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserItems_Users_UserId",
                table: "UserItems");

            migrationBuilder.DropTable(
                name: "SkinAndCharacterBundles");

            migrationBuilder.DropTable(
                name: "UserBanLogs");

            migrationBuilder.RenameColumn(
                name: "SkinAndCharacterBundleId",
                table: "UserBundles",
                newName: "ItemBundleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBundles_SkinAndCharacterBundleId",
                table: "UserBundles",
                newName: "IX_UserBundles_ItemBundleId");

            migrationBuilder.RenameColumn(
                name: "SkinAndCharacterBundleId",
                table: "ShopOrderDetails",
                newName: "ItemBundleId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopOrderDetails_SkinAndCharacterBundleId",
                table: "ShopOrderDetails",
                newName: "IX_ShopOrderDetails_ItemBundleId");

            migrationBuilder.RenameColumn(
                name: "SkinAndCharacterBundleId",
                table: "BundleItems",
                newName: "BundleId");

            migrationBuilder.CreateTable(
                name: "ItemBundles",
                columns: table => new
                {
                    ItemBundleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BundleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BundlePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBundles", x => x.ItemBundleId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BundleItems_ItemBundles_BundleId",
                table: "BundleItems",
                column: "BundleId",
                principalTable: "ItemBundles",
                principalColumn: "ItemBundleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrderDetails_ItemBundles_ItemBundleId",
                table: "ShopOrderDetails",
                column: "ItemBundleId",
                principalTable: "ItemBundles",
                principalColumn: "ItemBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrderDetails_ShopOrders_ShopOrderId",
                table: "ShopOrderDetails",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "ShopOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBundles_ItemBundles_ItemBundleId",
                table: "UserBundles",
                column: "ItemBundleId",
                principalTable: "ItemBundles",
                principalColumn: "ItemBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_ShopOrders_ShopOrderId",
                table: "UserItems",
                column: "ShopOrderId",
                principalTable: "ShopOrders",
                principalColumn: "ShopOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserItems_Users_UserId",
                table: "UserItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
