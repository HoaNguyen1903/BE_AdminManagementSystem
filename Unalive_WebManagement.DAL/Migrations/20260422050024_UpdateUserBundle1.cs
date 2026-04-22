using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBundle1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBundles",
                table: "UserBundles");

            migrationBuilder.AlterColumn<int>(
                name: "GemBundleId",
                table: "UserBundles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SkinAndCharacterBundleId",
                table: "UserBundles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserBundleId",
                table: "UserBundles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBundles",
                table: "UserBundles",
                column: "UserBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBundles_UserId",
                table: "UserBundles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBundles",
                table: "UserBundles");

            migrationBuilder.DropIndex(
                name: "IX_UserBundles_UserId",
                table: "UserBundles");

            migrationBuilder.DropColumn(
                name: "UserBundleId",
                table: "UserBundles");

            migrationBuilder.AlterColumn<int>(
                name: "SkinAndCharacterBundleId",
                table: "UserBundles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GemBundleId",
                table: "UserBundles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBundles",
                table: "UserBundles",
                columns: new[] { "UserId", "SkinAndCharacterBundleId", "GemBundleId" });
        }
    }
}
