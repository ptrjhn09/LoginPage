using System.ComponentModel.DataAnnotations;

namespace EyeWearProduct.Model
{
    public class TrackingHistory
    {
        [Key]
        public int TrackingId { get; set; }
        public int OrderDetailId { get; set; } // FK
        public string? Status { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
