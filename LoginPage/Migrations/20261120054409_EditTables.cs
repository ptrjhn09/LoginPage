using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyeWearProduct.Migrations
{
    /// <inheritdoc />
    public partial class EditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDetaildId",
                table: "TrackingHistory");

            migrationBuilder.AddColumn<int>(
                name: "TrackingId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetaildId",
                table: "TrackingHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
