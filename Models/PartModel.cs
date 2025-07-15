namespace Service_Record.Models
{
    public class PartModel
    {
        public int PartId { get; set; }
        public string PartName { get; set; } = null!;
        public string? PartNumber { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
