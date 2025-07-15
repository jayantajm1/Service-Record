using Microsoft.EntityFrameworkCore;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Enums;



namespace Service_Record.DAL.Context;

public partial class ServiceRecordDbContext : DbContext
{
    public ServiceRecordDbContext()
    {
    }

    public ServiceRecordDbContext(DbContextOptions<ServiceRecordDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<ServiceRecord> ServiceRecords { get; set; }

    public virtual DbSet<ServiceRecordPart> ServiceRecordParts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasPostgresEnum<LogAction>();
        modelBuilder.HasPostgresEnum<LogAction>();
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("branches_pkey");

            entity.ToTable("branches", tb => tb.HasComment("Manages all bank branches that can be serviced."));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("parts_pkey");

            entity.ToTable("parts", tb => tb.HasComment("A master list of all serviceable parts."));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.QuotationId).HasName("quotations_pkey");

            entity.ToTable("quotations", tb => tb.HasComment("Stores quotations sent to bank branches."));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Details).HasComment("JSONB is used to flexibly store quotation line items.");
            entity.Property(e => e.QuotationDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Branch).WithMany(p => p.Quotations)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("quotations_branch_id_fkey");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Quotations)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("quotations_created_by_id_fkey");
        });

        modelBuilder.Entity<ServiceRecord>(entity =>
        {
            entity.HasKey(e => e.ServiceRecordId).HasName("service_records_pkey");

            entity.ToTable("service_records", tb => tb.HasComment("Core table for service activities performed by field staff."));

            entity.Property(e => e.BranchManagerSignature).HasComment("Can store a Base64 encoded image or a path to the stored signature file.");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.FieldStaffId).HasComment("FK to the user with the 'field_staff' role who performed the service.");
            entity.Property(e => e.ServiceDatetime).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Branch).WithMany(p => p.ServiceRecords)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("service_records_branch_id_fkey");

            entity.HasOne(d => d.FieldStaff).WithMany(p => p.ServiceRecords)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("service_records_field_staff_id_fkey");
        });

        modelBuilder.Entity<ServiceRecordPart>(entity =>
        {
            entity.HasKey(e => e.ServiceRecordPartId).HasName("service_record_parts_pkey");

            entity.ToTable("service_record_parts", tb => tb.HasComment("Links parts to a specific service record, detailing what was added/removed/replaced."));

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Part).WithMany(p => p.ServiceRecordParts)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("service_record_parts_part_id_fkey");

            entity.HasOne(d => d.ServiceRecord).WithMany(p => p.ServiceRecordParts).HasConstraintName("service_record_parts_service_record_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users", tb => tb.HasComment("Stores all user accounts for the system."));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasComment("Should store a bcrypt or Argon2 hash of the password.");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("user_logs_pkey");

            entity.ToTable("user_logs", tb => tb.HasComment("Tracks login and logout events for all users."));

            entity.Property(e => e.LogTime).HasDefaultValueSql("now()");

           
            entity.Property(e => e.Action).HasColumnName("action");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.UserLogs)
                  .HasConstraintName("user_logs_user_id_fkey");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
