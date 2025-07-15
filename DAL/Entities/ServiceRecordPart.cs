
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Service_Record.DAL.Entities;

/// <summary>
/// Links parts to a specific service record, detailing what was added/removed/replaced.
/// </summary>
[Table("service_record_parts")]
public partial class ServiceRecordPart
{
    [Key]
    [Column("service_record_part_id")]
    public int ServiceRecordPartId { get; set; }

    [Column("service_record_id")]
    public int ServiceRecordId { get; set; }

    [Column("part_id")]
    public int PartId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [ForeignKey("PartId")]
    [InverseProperty("ServiceRecordParts")]
    public virtual Part Part { get; set; } = null!;

    [ForeignKey("ServiceRecordId")]
    [InverseProperty("ServiceRecordParts")]
    public virtual ServiceRecord ServiceRecord { get; set; } = null!;
}
