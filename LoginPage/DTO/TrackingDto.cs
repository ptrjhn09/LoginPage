namespace EyeWearProduct.DTO
{
    public class TrackingDto
    {
        public int TrackingId { get; set; }
        public string? Status { get; set; }
        public int OrderDetailId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime StatusDate { get; set; }


    }
}
