namespace EyeWearProduct.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public int ProductDetailId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        //public ProductDetails? ProductDetails { get; set; }
        //public Category? Category { get; set; }
    }
}
