namespace EyeWearProduct.Model
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public string? CustomerName { get; set; }
        public int Contact { get; set; }

        public string? Address { get; set; }
        public long TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }


    }
}
