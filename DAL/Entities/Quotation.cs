using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service_Record.DAL.Entities;

/// <summary>
/// Stores quotations sent to bank branches.
/// </summary>
[Table("quotations")]
[Index("BranchId", Name = "idx_quotations_branch_id")]
[Index("QuotationDate", Name = "idx_quotations_quotation_date")]
public partial class Quotation
{
    [Key]
    [Column("quotation_id")]
    public int QuotationId { get; set; }

    [Column("branch_id")]
    public int BranchId { get; set; }

    [Column("created_by_id")]
    public int CreatedById { get; set; }

    [Column("quotation_date")]
    public DateOnly QuotationDate { get; set; }

    /// <summary>
    /// JSONB is used to flexibly store quotation line items.
    /// </summary>
    [Column("details", TypeName = "jsonb")]
    public string? Details { get; set; }

    [Column("total_amount")]
    [Precision(10, 2)]
    public decimal TotalAmount { get; set; }

    [Column("sent_at")]
    public DateTime? SentAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("Quotations")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("CreatedById")]
    [InverseProperty("Quotations")]
    public virtual User CreatedBy { get; set; } = null!;
}
