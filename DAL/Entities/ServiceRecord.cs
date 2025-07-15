using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service_Record.DAL.Entities;

/// <summary>
/// Core table for service activities performed by field staff.
/// </summary>
[Table("service_records")]
[Index("BranchId", Name = "idx_service_records_branch_id")]
[Index("ServiceDatetime", Name = "idx_service_records_service_datetime")]
public partial class ServiceRecord
{
    [Key]
    [Column("service_record_id")]
    public int ServiceRecordId { get; set; }

    [Column("branch_id")]
    public int BranchId { get; set; }

    /// <summary>
    /// FK to the user with the &apos;field_staff&apos; role who performed the service.
    /// </summary>
    [Column("field_staff_id")]
    public int FieldStaffId { get; set; }

    [Column("service_datetime")]
    public DateTime? ServiceDatetime { get; set; }

    [Column("details")]
    public string Details { get; set; } = null!;

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    /// <summary>
    /// Can store a Base64 encoded image or a path to the stored signature file.
    /// </summary>
    [Column("branch_manager_signature")]
    public string? BranchManagerSignature { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BranchId")]
    [InverseProperty("ServiceRecords")]
    public virtual Branch Branch { get; set; } = null!;

    [ForeignKey("FieldStaffId")]
    [InverseProperty("ServiceRecords")]
    public virtual User FieldStaff { get; set; } = null!;

    [InverseProperty("ServiceRecord")]
    public virtual ICollection<ServiceRecordPart> ServiceRecordParts { get; set; } = new List<ServiceRecordPart>();
}
