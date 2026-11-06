namespace EyeWearProduct.Model
{
    public class Order
    {
        public int orderId { get; set; }
        public int OrderDetailId { get; set; }
        public int ProductDetailsId { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
