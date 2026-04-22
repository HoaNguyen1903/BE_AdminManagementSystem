using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBundleKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BundleItems");

            //migrationBuilder.AddColumn<int>(
            //    name: "ItemId",
            //    table: "SkinAndCharacterBundles",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "Quantity",
            //    table: "SkinAndCharacterBundles",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "SkinAndCharacterBundles");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SkinAndCharacterBundles");

            migrationBuilder.CreateTable(
                name: "BundleItems",
                columns: table => new
                {
                    SkinAndCharacterBundleId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleItems", x => new { x.SkinAndCharacterBundleId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BundleItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BundleItems_SkinAndCharacterBundles_SkinAndCharacterBundleId",
                        column: x => x.SkinAndCharacterBundleId,
                        principalTable: "SkinAndCharacterBundles",
                        principalColumn: "SkinAndCharacterBundleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BundleItems_ItemId",
                table: "BundleItems",
                column: "ItemId");
        }
    }
}
