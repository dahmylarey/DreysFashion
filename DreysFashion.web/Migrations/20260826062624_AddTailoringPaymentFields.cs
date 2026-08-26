using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreysFashion.web.Migrations
{
    /// <inheritdoc />
    public partial class AddTailoringPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "TailoringRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReference",
                table: "TailoringRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "TailoringRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "TailoringRequests");

            migrationBuilder.DropColumn(
                name: "PaymentReference",
                table: "TailoringRequests");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "TailoringRequests");
        }
    }
}
