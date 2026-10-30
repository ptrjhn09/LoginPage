namespace EyeWearProduct.Model
{
    public class ProductDetailsDto
    {
        public int ProductDetailId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public string? ProductCategory { get; set; }
        public int CategoryId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
