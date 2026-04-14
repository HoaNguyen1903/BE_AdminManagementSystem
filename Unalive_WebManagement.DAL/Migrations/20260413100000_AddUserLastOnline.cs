using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Unalive_WebManagement.Data;

#nullable disable

namespace Unalive_WebManagement.DAL.Migrations
{
    [DbContext(typeof(UnaliveDbContext))]
    [Migration("20260413100000_AddUserLastOnline")]
    public partial class AddUserLastOnline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastOnline",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastOnline",
                table: "Users");
        }
    }
}
