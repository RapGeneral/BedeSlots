using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class addedbalancetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Balances",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BalanceType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_TypeId",
                table: "Balances",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BalanceType_TypeId",
                table: "Balances",
                column: "TypeId",
                principalTable: "BalanceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BalanceType_TypeId",
                table: "Balances");

            migrationBuilder.DropTable(
                name: "BalanceType");

            migrationBuilder.DropIndex(
                name: "IX_Balances_TypeId",
                table: "Balances");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Balances");
        }
    }
}
