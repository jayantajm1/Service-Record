using Microsoft.EntityFrameworkCore;
using Service_Record.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service_Record.DAL.Entities;

/// <summary>
/// Stores all user accounts for the system.
/// </summary>
[Table("users")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
[Index("Username", Name = "users_username_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Should store a bcrypt or Argon2 hash of the password.
    /// </summary>
    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("role")]
    public UserRole Role { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("CreatedBy")]
    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    [InverseProperty("FieldStaff")]
    public virtual ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();

    [InverseProperty("User")]
    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
}
