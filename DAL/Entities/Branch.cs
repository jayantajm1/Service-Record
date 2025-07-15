using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service_Record.DAL.Entities;

/// <summary>
/// Manages all bank branches that can be serviced.
/// </summary>
[Table("branches")]
[Index("BranchCode", Name = "branches_branch_code_key", IsUnique = true)]
public partial class Branch
{
    [Key]
    [Column("branch_id")]
    public int BranchId { get; set; }

    [Column("branch_name")]
    [StringLength(150)]
    public string BranchName { get; set; } = null!;

    [Column("branch_code")]
    [StringLength(50)]
    public string BranchCode { get; set; } = null!;

    [Column("address")]
    public string? Address { get; set; }

    [Column("registered_email")]
    [StringLength(100)]
    public string RegisteredEmail { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    [InverseProperty("Branch")]
    public virtual ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();
}
