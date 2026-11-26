namespace EyeWearProduct.DTO
{
    public class OrderItemDto
    {
        public int OrderDetailId { get; set; }
        public string? CustomerName { get; set; }
        public int Contact { get; set; }
        public string? Address { get; set; }
        public int Price { get; set; }

        public long TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }

        public string? productName { get; set; }
        public string? imageUrl { get; set; }

        public string? Status { get; set; }
    }

}
