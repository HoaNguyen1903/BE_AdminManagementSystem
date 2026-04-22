using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBundle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GemBundleId1",
                table: "UserBundles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkinAndCharacterBundleId1",
                table: "UserBundles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserBundles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_GemBundleId1",
                table: "UserBundles",
                column: "GemBundleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_SkinAndCharacterBundleId1",
                table: "UserBundles",
                column: "SkinAndCharacterBundleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_UserId1",
                table: "UserBundles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBundles_GemBundles_GemBundleId1",
                table: "UserBundles",
                column: "GemBundleId1",
                principalTable: "GemBundles",
                principalColumn: "GemBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBundles_SkinAndCharacterBundles_SkinAndCharacterBundleId1",
                table: "UserBundles",
                column: "SkinAndCharacterBundleId1",
                principalTable: "SkinAndCharacterBundles",
                principalColumn: "SkinAndCharacterBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBundles_Users_UserId1",
                table: "UserBundles",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBundles_GemBundles_GemBundleId1",
                table: "UserBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBundles_SkinAndCharacterBundles_SkinAndCharacterBundleId1",
                table: "UserBundles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBundles_Users_UserId1",
                table: "UserBundles");

            migrationBuilder.DropIndex(
                name: "IX_UserBundles_GemBundleId1",
                table: "UserBundles");

            migrationBuilder.DropIndex(
                name: "IX_UserBundles_SkinAndCharacterBundleId1",
                table: "UserBundles");

            migrationBuilder.DropIndex(
                name: "IX_UserBundles_UserId1",
                table: "UserBundles");

            migrationBuilder.DropColumn(
                name: "GemBundleId1",
                table: "UserBundles");

            migrationBuilder.DropColumn(
                name: "SkinAndCharacterBundleId1",
                table: "UserBundles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserBundles");
        }
    }
}
