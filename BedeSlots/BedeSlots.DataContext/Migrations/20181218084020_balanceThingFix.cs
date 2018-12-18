using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class balanceThingFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BalanceTypes_TypeId",
                table: "Balances");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Balances",
                newName: "TypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Balances_TypeId",
                table: "Balances",
                newName: "IX_Balances_TypeID");

            migrationBuilder.AlterColumn<Guid>(
                name: "TypeID",
                table: "Balances",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BalanceTypes_TypeID",
                table: "Balances",
                column: "TypeID",
                principalTable: "BalanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BalanceTypes_TypeID",
                table: "Balances");

            migrationBuilder.RenameColumn(
                name: "TypeID",
                table: "Balances",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Balances_TypeID",
                table: "Balances",
                newName: "IX_Balances_TypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TypeId",
                table: "Balances",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BalanceTypes_TypeId",
                table: "Balances",
                column: "TypeId",
                principalTable: "BalanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
