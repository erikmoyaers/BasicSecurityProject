using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BasicSecurityProject.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PrivateKey",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PublicKey",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PublicKey",
                table: "Accounts");
        }
    }
}
