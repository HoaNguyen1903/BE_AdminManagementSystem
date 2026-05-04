using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addPurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "SkinAndCharacterBundles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImageUrl",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "GemBundles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CharacterPvPs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "SkinAndCharacterBundles");

            migrationBuilder.DropColumn(
                name: "ItemImageUrl",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "GemBundles");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CharacterPvPs");
        }
    }
}
