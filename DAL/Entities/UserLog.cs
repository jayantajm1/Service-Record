using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Service_Record.DAL.Enums; // Assuming log_action is defined here

namespace Service_Record.DAL.Entities;

/// <summary>
/// Tracks login and logout events for all users.
/// </summary>
[Table("user_logs")]
[Index("UserId", Name = "idx_user_logs_user_id")]
public partial class UserLog
{
    [Key]
    [Column("log_id")]
    public long LogId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("action")]
    public LogAction Action { get; set; } 

    [Column("log_time")]
    public DateTime? LogTime { get; set; }

    [Column("ip_address")]
    [StringLength(45)]
    public string? IpAddress { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserLogs")]
    public virtual User User { get; set; } = null!;
}
