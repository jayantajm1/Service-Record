namespace Service_Record.Models
{
    public class BranchModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string BranchCode { get; set; } = null!;
        public string? Address { get; set; }
        public string RegisteredEmail { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
