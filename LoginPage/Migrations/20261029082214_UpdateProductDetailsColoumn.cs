using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyeWearProduct.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductDetailsColoumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ProductDetails");
        }
    }
}
