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

    public virtual DbSet<AgeUnit> AgeUnits { get; set; }

    public virtual DbSet<AppointmentService> AppointmentServices { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<IntervalUnit> IntervalUnits { get; set; }

    public virtual DbSet<ParentChild> ParentChildren { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

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
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267251E4556D029");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
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

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__roleId__4D94879B");
        });

        modelBuilder.Entity<AgeUnit>(entity =>
        {
            entity.HasKey(e => e.AgeUnitId).HasName("PK__AgeUnit__ABBA3B182D10E44F");

            entity.ToTable("AgeUnit");

            entity.Property(e => e.AgeUnitId)
                .ValueGeneratedNever()
                .HasColumnName("ageUnitId");
            entity.Property(e => e.AgeUnitName)
                .HasMaxLength(10)
                .HasColumnName("ageUnitName");
        });

        modelBuilder.Entity<AppointmentService>(entity =>
        {
            entity.HasKey(e => e.AppointmentServiceId).HasName("PK__Appointm__317F8CDB90C6880C");

            entity.ToTable("Appointment_Service");

            entity.Property(e => e.AppointmentServiceId).HasColumnName("appointmentServiceId");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentServices)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__appoi__797309D9");

            entity.HasOne(d => d.Service).WithMany(p => p.AppointmentServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__servi__7A672E12");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__BD58D473FFDDABE2");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("bannerId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.BannerName)
                .HasMaxLength(255)
                .HasColumnName("bannerName");

            entity.HasOne(d => d.Account).WithMany(p => p.Banners)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Banner__accountI__09A971A2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D8692B0687");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__Configur__3FEDA8E6DB72C6C7");

            entity.ToTable("Configuration");

            entity.Property(e => e.ConfigId).HasColumnName("configId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(255)
                .HasColumnName("facilityName");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseaseId).HasName("PK__Disease__49A195571DA2B259");

            entity.ToTable("Disease");

            entity.Property(e => e.DiseaseId).HasColumnName("diseaseId");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.DiseaseName)
                .HasMaxLength(255)
                .HasColumnName("diseaseName");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC47C975A2F");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("comment");
            entity.Property(e => e.FeedbackDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("feedbackDate");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Account).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Feedback__accoun__03F0984C");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Feedback__appoin__02FC7413");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__Image__C5BC81A6041E7745");

            entity.ToTable("Image");

            entity.Property(e => e.ImgId)
                .ValueGeneratedNever()
                .HasColumnName("imgId");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img");

            entity.HasMany(d => d.Vaccinations).WithMany(p => p.Imgs)
                .UsingEntity<Dictionary<string, object>>(
                    "VaccinationImage",
                    r => r.HasOne<Vaccination>().WithMany()
                        .HasForeignKey("VaccinationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vaccinati__vacci__6754599E"),
                    l => l.HasOne<Image>().WithMany()
                        .HasForeignKey("ImgId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vaccinati__imgId__66603565"),
                    j =>
                    {
                        j.HasKey("ImgId", "VaccinationId").HasName("PK__Vaccinat__4E482FC68D256BE6");
                        j.ToTable("Vaccination_Image");
                        j.IndexerProperty<int>("ImgId").HasColumnName("imgId");
                        j.IndexerProperty<int>("VaccinationId").HasColumnName("vaccinationId");
                    });
        });

        modelBuilder.Entity<IntervalUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Interval__55D79235F95A1965");

            entity.ToTable("IntervalUnit");

            entity.Property(e => e.UnitId)
                .ValueGeneratedNever()
                .HasColumnName("unitId");
            entity.Property(e => e.UnitName)
                .HasMaxLength(10)
                .HasColumnName("unitName");
        });

        modelBuilder.Entity<ParentChild>(entity =>
        {
            entity.HasKey(e => e.ParentPatientId).HasName("PK__Parent_C__02E0DC71BF869697");

            entity.ToTable("Parent_Child");

            entity.Property(e => e.ParentPatientId).HasColumnName("parent_patient_id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.PatientId).HasColumnName("patientId");

            entity.HasOne(d => d.Account).WithMany(p => p.ParentChildren)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent_Ch__accou__5535A963");

            entity.HasOne(d => d.Patient).WithMany(p => p.ParentChildren)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent_Ch__patie__5629CD9C");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__A17005EC8F6C0F47");

            entity.ToTable("Patient");

            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.GuardianPhone)
                .HasMaxLength(20)
                .HasColumnName("guardianPhone");
            entity.Property(e => e.PatientName)
                .HasMaxLength(255)
                .HasColumnName("patientName");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RelationshipToAccount)
                .HasMaxLength(50)
                .HasColumnName("relationshipToAccount");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA63216D0F9");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.Status)
                .HasMaxLength(1000)
                .HasColumnName("status");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__appoint__06CD04F7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A88B60A07");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrafficL__3213E83FC4EA63B6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ipAddress");
            entity.Property(e => e.IsRegistered).HasColumnName("isRegistered");
            entity.Property(e => e.RequestMethod)
                .HasMaxLength(10)
                .HasColumnName("requestMethod");
            entity.Property(e => e.RequestPath)
                .HasMaxLength(255)
                .HasColumnName("requestPath");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(255)
                .HasColumnName("userAgent");

            entity.HasOne(d => d.Account).WithMany(p => p.TrafficLogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__TrafficLo__accou__5070F446");
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE609C5059D46");

            entity.ToTable("Vaccination");

            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");
            entity.Property(e => e.AgeUnitId).HasColumnName("ageUnitId");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Interval).HasColumnName("interval");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(255)
                .HasColumnName("manufacturer");
            entity.Property(e => e.MaxAge).HasColumnName("maxAge");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.TotalDoses).HasColumnName("totalDoses");
            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.VaccinationName)
                .HasMaxLength(255)
                .HasColumnName("vaccinationName");

            entity.HasOne(d => d.AgeUnit).WithMany(p => p.Vaccinations)
                .HasForeignKey(d => d.AgeUnitId)
                .HasConstraintName("FK__Vaccinati__ageUn__628FA481");

            entity.HasOne(d => d.Unit).WithMany(p => p.Vaccinations)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Vaccinati__unitI__6383C8BA");

            entity.HasMany(d => d.Diseases).WithMany(p => p.Vaccinations)
                .UsingEntity<Dictionary<string, object>>(
                    "VaccinationDisease",
                    r => r.HasOne<Disease>().WithMany()
                        .HasForeignKey("DiseaseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vaccinati__disea__6B24EA82"),
                    l => l.HasOne<Vaccination>().WithMany()
                        .HasForeignKey("VaccinationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Vaccinati__vacci__6A30C649"),
                    j =>
                    {
                        j.HasKey("VaccinationId", "DiseaseId").HasName("PK__Vaccinat__DBD0FF5C5395F9CB");
                        j.ToTable("Vaccination_Disease");
                        j.IndexerProperty<int>("VaccinationId").HasColumnName("vaccinationId");
                        j.IndexerProperty<int>("DiseaseId").HasColumnName("diseaseId");
                    });
        });

        modelBuilder.Entity<VaccinationAppointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Vaccinat__D067651E7E062855");

            entity.ToTable("Vaccination_Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointmentDate");
            entity.Property(e => e.AppointmentNumber).HasColumnName("appointmentNumber");
            entity.Property(e => e.ConfigId).HasColumnName("configId");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Account).WithMany(p => p.VaccinationAppointments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__accou__76969D2E");

            entity.HasOne(d => d.Config).WithMany(p => p.VaccinationAppointments)
                .HasForeignKey(d => d.ConfigId)
                .HasConstraintName("FK__Vaccinati__confi__75A278F5");

            entity.HasOne(d => d.Patient).WithMany(p => p.VaccinationAppointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Vaccinati__patie__74AE54BC");
        });

        modelBuilder.Entity<VaccinationHistory>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE669FB60CE12");

            entity.ToTable("Vaccination_History");

            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationID");
            entity.Property(e => e.ConfigId).HasColumnName("configId");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patientID");
            entity.Property(e => e.Reaction)
                .HasMaxLength(500)
                .HasColumnName("reaction");
            entity.Property(e => e.VaccinationDate).HasColumnName("vaccinationDate");
            entity.Property(e => e.VaccineId).HasColumnName("vaccineId");

            entity.HasOne(d => d.Config).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.ConfigId)
                .HasConstraintName("FK__Vaccinati__confi__7F2BE32F");

            entity.HasOne(d => d.Patient).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Vaccinati__patie__7D439ABD");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__7E37BEF6");
        });

        modelBuilder.Entity<VaccinationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Vaccinat__4550733FC5B87428");

            entity.ToTable("Vaccination_Service");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
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

            entity.HasOne(d => d.Category).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Vaccinati__categ__70DDC3D8");

            entity.HasOne(d => d.Disease).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.DiseaseId)
                .HasConstraintName("FK__Vaccinati__disea__71D1E811");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Vaccinati__vacci__6FE99F9F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
