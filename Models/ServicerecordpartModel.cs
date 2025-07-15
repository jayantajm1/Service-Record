namespace Service_Record.Models
{
    public class ServicerecordpartModel
    {
        public int ServiceRecordPartId { get; set; }
        public int ServiceRecordId { get; set; }
        public int PartId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
