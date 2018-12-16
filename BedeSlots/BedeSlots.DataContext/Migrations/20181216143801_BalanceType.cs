using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class BalanceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BalanceType_TypeId",
                table: "Balances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BalanceType",
                table: "BalanceType");

            migrationBuilder.RenameTable(
                name: "BalanceType",
                newName: "BalanceTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BalanceTypes",
                table: "BalanceTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BalanceTypes_TypeId",
                table: "Balances",
                column: "TypeId",
                principalTable: "BalanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_BalanceTypes_TypeId",
                table: "Balances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BalanceTypes",
                table: "BalanceTypes");

            migrationBuilder.RenameTable(
                name: "BalanceTypes",
                newName: "BalanceType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BalanceType",
                table: "BalanceType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_BalanceType_TypeId",
                table: "Balances",
                column: "TypeId",
                principalTable: "BalanceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
