using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.DataContext.Migrations
{
    public partial class BalanceTypeAdditional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
