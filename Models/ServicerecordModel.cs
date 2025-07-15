namespace Service_Record.Models
{
    public class ServicerecordModel
    {
        public int ServiceRecordId { get; set; }
        public int BranchId { get; set; }
        public int FieldStaffId { get; set; }
        public DateTime? ServiceDatetime { get; set; }
        public string Details { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? BranchManagerSignature { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
