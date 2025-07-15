using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service_Record.DAL.Entities;

/// <summary>
/// A master list of all serviceable parts.
/// </summary>
[Table("parts")]
[Index("PartNumber", Name = "parts_part_number_key", IsUnique = true)]
public partial class Part
{
    [Key]
    [Column("part_id")]
    public int PartId { get; set; }

    [Column("part_name")]
    [StringLength(100)]
    public string PartName { get; set; } = null!;

    [Column("part_number")]
    [StringLength(50)]
    public string? PartNumber { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Part")]
    public virtual ICollection<ServiceRecordPart> ServiceRecordParts { get; set; } = new List<ServiceRecordPart>();
}
