using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.DataAccess.Data;

public partial class VaccinationTrackingContext : DbContext
{
    public VaccinationTrackingContext()
    {
    }

    public VaccinationTrackingContext(DbContextOptions<VaccinationTrackingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Child> Children { get; set; }

    public virtual DbSet<OrderVaccine> OrderVaccines { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Vaccination> Vaccinations { get; set; }

    public virtual DbSet<VaccinationAppointment> VaccinationAppointments { get; set; }

    public virtual DbSet<VaccinationHistory> VaccinationHistories { get; set; }

    public virtual DbSet<VaccinationService> VaccinationServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-KG3S19MT;Database=VaccinationTracking;User Id=sa;Password=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267251EBA938449");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E6164ED24C9A1").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Account__B43B145FB48727ED").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.ChildId).HasColumnName("childId");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Child).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__Account__childId__5070F446");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__roleId__4F7CD00D");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D82F8EFF5A");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Child>(entity =>
        {
            entity.HasKey(e => e.ChildId).HasName("PK__Child__223925CDD274CEF6");

            entity.ToTable("Child");

            entity.Property(e => e.ChildId).HasColumnName("childId");
            entity.Property(e => e.ChildName)
                .HasMaxLength(255)
                .HasColumnName("childName");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.VaccinationStatus)
                .HasMaxLength(255)
                .HasColumnName("vaccinationStatus");
        });

        modelBuilder.Entity<OrderVaccine>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order_Va__0809335D44D1C0F6");

            entity.ToTable("Order_Vaccines");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.OrderVaccines)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Order_Vac__vacci__6477ECF3");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA685277A37");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.Status)
                .HasMaxLength(1000)
                .HasColumnName("status");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__appoint__619B8048");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A2C444146");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE609BBE85477");

            entity.ToTable("Vaccination");

            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.VaccinationName)
                .HasMaxLength(255)
                .HasColumnName("vaccinationName");
        });

        modelBuilder.Entity<VaccinationAppointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Vaccinat__D067651E7BE64269");

            entity.ToTable("Vaccination_Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointmentDate");
            entity.Property(e => e.ChildId).HasColumnName("childID");
            entity.Property(e => e.ScheduledDate).HasColumnName("scheduledDate");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.Child).WithMany(p => p.VaccinationAppointments)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__Vaccinati__child__534D60F1");
        });

        modelBuilder.Entity<VaccinationHistory>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE669D30E53E8");

            entity.ToTable("Vaccination_History");

            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationID");
            entity.Property(e => e.ChildId).HasColumnName("childID");
            entity.Property(e => e.FacilityId).HasColumnName("facilityID");
            entity.Property(e => e.Reaction)
                .HasMaxLength(500)
                .HasColumnName("reaction");
            entity.Property(e => e.VaccinationDate).HasColumnName("vaccinationDate");
            entity.Property(e => e.VaccineId).HasColumnName("vaccineId");

            entity.HasOne(d => d.Child).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.ChildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__child__5629CD9C");
        });

        modelBuilder.Entity<VaccinationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Vaccinat__4550733F16E5CEE8");

            entity.ToTable("Vaccination_Service");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.DiseaseId).HasColumnName("diseaseId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("serviceName");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Appointment).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Vaccinati__appoi__5EBF139D");

            entity.HasOne(d => d.Category).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Vaccinati__categ__5DCAEF64");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Vaccinati__vacci__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
