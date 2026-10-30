using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyeWearProduct.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryFromProductDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ProductDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
