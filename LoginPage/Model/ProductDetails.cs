using System.ComponentModel.DataAnnotations;

namespace EyeWearProduct.Model


{
    public class ProductDetails
    {
        [Key]
        public int ProductDetailId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }

        public  DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }




       //public ICollection<Product>? Products { get; set; }
    }
}
    