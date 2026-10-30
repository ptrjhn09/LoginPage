using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EyeWearProduct.Migrations
{
    /// <inheritdoc />
    public partial class RedoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDetails_ProductDetailsProductDetailId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductDetailsProductDetailId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDetailsProductDetailId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDetailsProductDetailId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductDetailsProductDetailId",
                table: "Products",
                column: "ProductDetailsProductDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDetails_ProductDetailsProductDetailId",
                table: "Products",
                column: "ProductDetailsProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "ProductDetailId");
        }
    }
}
