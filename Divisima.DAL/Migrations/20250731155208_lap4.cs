using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Divisima.DAL.Migrations
{
    /// <inheritdoc />
    public partial class lap4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "ID",
                keyValue: 1,
                column: "LastLoginDate",
                value: new DateTime(2025, 7, 31, 18, 52, 6, 647, DateTimeKind.Local).AddTicks(6226));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "ID",
                keyValue: 1,
                column: "LastLoginDate",
                value: new DateTime(2025, 7, 17, 19, 13, 6, 650, DateTimeKind.Local).AddTicks(423));
        }
    }
}
