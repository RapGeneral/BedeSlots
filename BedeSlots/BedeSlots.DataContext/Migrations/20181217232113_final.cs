using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_UserId",
                table: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("87f5cab8-0031-4d38-8480-4ba76a8f4a06"));

            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("c674878f-9008-48cb-a826-c17d8ad7dc04"));

            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("d0541f8a-ab80-4317-b4fc-cc920007863b"));

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionTypes",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                table: "Currencies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "BankDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BalanceTypes",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Balances",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_UserId",
                table: "Balances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_AspNetUsers_UserId",
                table: "Balances");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                table: "Currencies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "BankDetails",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BalanceTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Balances",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c674878f-9008-48cb-a826-c17d8ad7dc04"), "Personal" });

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d0541f8a-ab80-4317-b4fc-cc920007863b"), "Base" });

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("87f5cab8-0031-4d38-8480-4ba76a8f4a06"), "Bonus" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrencyId",
                table: "AspNetUsers",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_CurrencyId",
                table: "AspNetUsers",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_AspNetUsers_UserId",
                table: "Balances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
