using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyeWearProduct.Migrations
{
    /// <inheritdoc />
    public partial class AddModifyDateinOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Orders");
        }
    }
}
