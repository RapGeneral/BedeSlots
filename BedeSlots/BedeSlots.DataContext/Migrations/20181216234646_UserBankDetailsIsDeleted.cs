using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class UserBankDetailsIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("706fbf4e-0b30-4d90-b542-b0c192e3d547"));

            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("8426f5d3-038f-4cb0-9df0-c43d3644d71d"));

            migrationBuilder.DeleteData(
                table: "BalanceTypes",
                keyColumn: "Id",
                keyValue: new Guid("ebbe0c6a-5fe2-4a5f-972c-bd1fe7c83a46"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserBankDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserBankDetails",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "DeletedOn",
                table: "UserBankDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserBankDetails");

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("ebbe0c6a-5fe2-4a5f-972c-bd1fe7c83a46"), "Personal" });

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("706fbf4e-0b30-4d90-b542-b0c192e3d547"), "Base" });

            migrationBuilder.InsertData(
                table: "BalanceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8426f5d3-038f-4cb0-9df0-c43d3644d71d"), "Bonus" });
        }
    }
}
