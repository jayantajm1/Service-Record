namespace Service_Record.Models
{
    public class QuotationModel
    {
        public int QuotationId { get; set; }
        public int BranchId { get; set; }
        public int CreatedById { get; set; }
        public DateOnly QuotationDate { get; set; }
        public string? Details { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
