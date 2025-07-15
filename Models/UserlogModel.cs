namespace Service_Record.Models
{
    public class UserlogModel
    {
        public long LogId { get; set; }
        public int UserId { get; set; }
        public DateTime? LogTime { get; set; }
        public string? IpAddress { get; set; }
    }
}
