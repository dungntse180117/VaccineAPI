using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Child> Children { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<OrderVaccine> OrderVaccines { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TrafficLog> TrafficLogs { get; set; }

    public virtual DbSet<Vaccination> Vaccinations { get; set; }

    public virtual DbSet<VaccinationAppointment> VaccinationAppointments { get; set; }

    public virtual DbSet<VaccinationHistory> VaccinationHistories { get; set; }

    public virtual DbSet<VaccinationService> VaccinationServices { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267251E9CC77203");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.ChildId).HasColumnName("childId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Child).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ChildId)
                .HasConstraintName("FK__Account__childId__5070F446");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__roleId__4F7CD00D");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__BD58D473926C7F6C");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("bannerId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.BannerImage)
                .HasMaxLength(255)
                .HasColumnName("bannerImage");
            entity.Property(e => e.BannerName)
                .HasMaxLength(255)
                .HasColumnName("bannerName");

            entity.HasOne(d => d.Account).WithMany(p => p.Banners)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Banner__accountI__534D60F1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D8713B6BF7");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Child>(entity =>
        {
            entity.HasKey(e => e.ChildId).HasName("PK__Child__223925CD024B4100");

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

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC4685A87E3");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("comment");
            entity.Property(e => e.FeedbackDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("feedbackDate");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Feedback__appoin__6754599E");

            entity.HasOne(d => d.Service).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Feedback__servic__68487DD7");
        });

        modelBuilder.Entity<OrderVaccine>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order_Va__0809335D070C3B78");

            entity.ToTable("Order_Vaccines");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.OrderVaccines)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Order_Vac__vacci__70DDC3D8");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA6B5122AE0");

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
                .HasConstraintName("FK__Payment__appoint__6B24EA82");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailId).HasName("PK__Payment___A04B61BA833D0D6D");

            entity.ToTable("Payment_Detail");

            entity.Property(e => e.PaymentDetailId).HasColumnName("paymentDetailID");
            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment_D__payme__6E01572D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A08A6AF82");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrafficL__3214EC070BF57E20");

            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.RequestMethod).HasMaxLength(10);
            entity.Property(e => e.RequestPath).HasMaxLength(255);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UserAgent).HasMaxLength(255);
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE609B517BBBC");

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
            entity.HasKey(e => e.AppointmentId).HasName("PK__Vaccinat__D067651E3D063B40");

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
                .HasConstraintName("FK__Vaccinati__child__5812160E");
        });

        modelBuilder.Entity<VaccinationHistory>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE669AE11433D");

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
                .HasConstraintName("FK__Vaccinati__child__5AEE82B9");
        });

        modelBuilder.Entity<VaccinationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Vaccinat__4550733FAC7D909C");

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
                .HasConstraintName("FK__Vaccinati__appoi__6383C8BA");

            entity.HasOne(d => d.Category).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Vaccinati__categ__628FA481");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Vaccinati__vacci__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
