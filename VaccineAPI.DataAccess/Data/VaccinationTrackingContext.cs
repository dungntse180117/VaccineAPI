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

    public virtual DbSet<AgeUnit> AgeUnits { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentVaccination> AppointmentVaccinations { get; set; }

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

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<RegistrationDetail> RegistrationDetails { get; set; }

    public virtual DbSet<RegistrationPatient> RegistrationPatients { get; set; }

    public virtual DbSet<RegistrationVaccination> RegistrationVaccinations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TrafficLog> TrafficLogs { get; set; }

    public virtual DbSet<Vaccination> Vaccinations { get; set; }

    public virtual DbSet<VaccinationDisease> VaccinationDiseases { get; set; }

    public virtual DbSet<VaccinationHistory> VaccinationHistories { get; set; }

    public virtual DbSet<VaccinationImage> VaccinationImages { get; set; }

    public virtual DbSet<VaccinationService> VaccinationServices { get; set; }

    public virtual DbSet<VaccinationServiceVaccination> VaccinationServiceVaccinations { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<VisitDayChangeRequest> VisitDayChangeRequests { get; set; }

    public virtual DbSet<VisitVaccination> VisitVaccinations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-1ULHS1KH;Database=VaccinationTracking;User Id=sa;Password=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267251EEF41EB73");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
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
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__roleId__38996AB5");
        });

        modelBuilder.Entity<AgeUnit>(entity =>
        {
            entity.HasKey(e => e.AgeUnitId).HasName("PK__AgeUnit__ABBA3B18995EF198");

            entity.ToTable("AgeUnit");

            entity.Property(e => e.AgeUnitId).HasColumnName("ageUnitId");
            entity.Property(e => e.AgeUnitName)
                .HasMaxLength(10)
                .HasColumnName("ageUnitName");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__D067651E400BDE54");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointmentDate");
            entity.Property(e => e.AppointmentNumber).HasColumnName("appointmentNumber");
            entity.Property(e => e.ConfigId).HasColumnName("configId");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.RegistrationDetailId).HasColumnName("registrationDetailID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Config).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ConfigId)
                .HasConstraintName("FK__Appointme__confi__72C60C4A");

            entity.HasOne(d => d.RegistrationDetail).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.RegistrationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__regis__71D1E811");
        });

        modelBuilder.Entity<AppointmentVaccination>(entity =>
        {
            entity.HasKey(e => e.AppointmentVaccinationId).HasName("PK__Appointm__F3AFE825737B975C");

            entity.ToTable("Appointment_Vaccination");

            entity.Property(e => e.AppointmentVaccinationId).HasColumnName("appointmentVaccinationID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.DosesRemaining).HasColumnName("dosesRemaining");
            entity.Property(e => e.DosesScheduled).HasColumnName("dosesScheduled");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalDoses).HasColumnName("totalDoses");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");
            entity.Property(e => e.VaccinationName)
                .HasMaxLength(255)
                .HasColumnName("vaccinationName");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentVaccinations)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__appoi__787EE5A0");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.AppointmentVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__vacci__797309D9");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__BD58D47315E82D08");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("bannerId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.BannerName)
                .HasMaxLength(255)
                .HasColumnName("bannerName");

            entity.HasOne(d => d.Account).WithMany(p => p.Banners)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Banner__accountI__10566F31");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D86649713D");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__Configur__3FEDA8E68825688C");

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
            entity.HasKey(e => e.DiseaseId).HasName("PK__Disease__49A1955755F1A782");

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
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC4B4F8C3D4");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("comment");
            entity.Property(e => e.FeedbackDate)
                .HasColumnType("datetime")
                .HasColumnName("feedbackDate");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.VaccinationHistoryId).HasColumnName("vaccinationHistoryID");

            entity.HasOne(d => d.Account).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Feedback__accoun__0A9D95DB");

            entity.HasOne(d => d.VaccinationHistory).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.VaccinationHistoryId)
                .HasConstraintName("FK__Feedback__vaccin__09A971A2");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__Image__C5BC81A60C052C50");

            entity.ToTable("Image");

            entity.Property(e => e.ImgId).HasColumnName("imgId");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img");
        });

        modelBuilder.Entity<IntervalUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Interval__55D792352091DD49");

            entity.ToTable("IntervalUnit");

            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.UnitName)
                .HasMaxLength(10)
                .HasColumnName("unitName");
        });

        modelBuilder.Entity<ParentChild>(entity =>
        {
            entity.HasKey(e => e.ParentPatientId).HasName("PK__Parent_C__02E0DC71E1D27AEE");

            entity.ToTable("Parent_Child");

            entity.Property(e => e.ParentPatientId).HasColumnName("parent_patient_id");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.PatientId).HasColumnName("patientId");

            entity.HasOne(d => d.Account).WithMany(p => p.ParentChildren)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent_Ch__accou__403A8C7D");

            entity.HasOne(d => d.Patient).WithMany(p => p.ParentChildren)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parent_Ch__patie__412EB0B6");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__A17005EC5D68350B");

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
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA6209D1CA7");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");
            entity.Property(e => e.Status)
                .HasMaxLength(1000)
                .HasColumnName("status");

            entity.HasOne(d => d.Registration).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__registr__0D7A0286");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__A3DB1415A945B309");

            entity.ToTable("Registration");

            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.DesiredDate)
                .HasColumnType("datetime")
                .HasColumnName("desiredDate");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("datetime")
                .HasColumnName("registrationDate");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.Account).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__accou__628FA481");

            entity.HasOne(d => d.Service).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Registrat__servi__6383C8BA");
        });

        modelBuilder.Entity<RegistrationDetail>(entity =>
        {
            entity.HasKey(e => e.RegistrationDetailId).HasName("PK__Registra__466FE99586B6C567");

            entity.ToTable("RegistrationDetail");

            entity.Property(e => e.RegistrationDetailId).HasColumnName("registrationDetailID");
            entity.Property(e => e.DesiredDate)
                .HasColumnType("datetime")
                .HasColumnName("desiredDate");
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Patient).WithMany(p => p.RegistrationDetails)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Registrat__patie__6EF57B66");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationDetails)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__6E01572D");
        });

        modelBuilder.Entity<RegistrationPatient>(entity =>
        {
            entity.HasKey(e => e.RegistrationPatientId).HasName("PK__Registra__335EAAC9CB0EBBDB");

            entity.ToTable("Registration_Patient");

            entity.Property(e => e.RegistrationPatientId).HasColumnName("registrationPatientID");
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");

            entity.HasOne(d => d.Patient).WithMany(p => p.RegistrationPatients)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__patie__6B24EA82");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationPatients)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__6A30C649");
        });

        modelBuilder.Entity<RegistrationVaccination>(entity =>
        {
            entity.HasKey(e => e.RegistrationPatientId).HasName("PK__Registra__335EAAC91F83249C");

            entity.ToTable("Registration_Vaccination");

            entity.Property(e => e.RegistrationPatientId).HasColumnName("registrationPatientID");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationVaccinations)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__66603565");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.RegistrationVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Registrat__vacci__6754599E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A5EDC6FE1");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrafficL__3213E83FB9FDB025");

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
                .HasConstraintName("FK__TrafficLo__accou__3B75D760");
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE609EEB09D00");

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
                .HasConstraintName("FK__Vaccinati__ageUn__4D94879B");

            entity.HasOne(d => d.Unit).WithMany(p => p.Vaccinations)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Vaccinati__unitI__4E88ABD4");
        });

        modelBuilder.Entity<VaccinationDisease>(entity =>
        {
            entity.HasKey(e => e.VaccinationDiseId).HasName("PK__Vaccinat__C7D778B0198B3E3A");

            entity.ToTable("Vaccination_Disease");

            entity.Property(e => e.VaccinationDiseId).HasColumnName("vaccinationDiseId");
            entity.Property(e => e.DiseaseId).HasColumnName("diseaseId");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Disease).WithMany(p => p.VaccinationDiseases)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__disea__571DF1D5");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationDiseases)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__5629CD9C");
        });

        modelBuilder.Entity<VaccinationHistory>(entity =>
        {
            entity.HasKey(e => e.VaccinationHistoryId).HasName("PK__Vaccinat__3FD342CADA1414CB");

            entity.ToTable("Vaccination_History");

            entity.Property(e => e.VaccinationHistoryId).HasColumnName("vaccinationHistoryID");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.Reaction)
                .HasMaxLength(500)
                .HasColumnName("reaction");
            entity.Property(e => e.VaccinationDate)
                .HasColumnType("datetime")
                .HasColumnName("vaccinationDate");
            entity.Property(e => e.VaccineId).HasColumnName("vaccineId");
            entity.Property(e => e.VisitId).HasColumnName("VisitID");

            entity.HasOne(d => d.Patient).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Vaccinati__patie__06CD04F7");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__05D8E0BE");

            entity.HasOne(d => d.Visit).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__Visit__04E4BC85");
        });

        modelBuilder.Entity<VaccinationImage>(entity =>
        {
            entity.HasKey(e => e.VaccinationImgId).HasName("PK__Vaccinat__058FD0623EB6E0B7");

            entity.ToTable("Vaccination_Image");

            entity.Property(e => e.VaccinationImgId).HasColumnName("vaccinationImgId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.ImgId).HasColumnName("imgId");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Account).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__accou__534D60F1");

            entity.HasOne(d => d.Img).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.ImgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__imgId__5165187F");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__52593CB8");
        });

        modelBuilder.Entity<VaccinationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Vaccinat__4550733FA864BE3D");

            entity.ToTable("VaccinationService");

            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("serviceName");
            entity.Property(e => e.TotalDoses).HasColumnName("totalDoses");

            entity.HasOne(d => d.Category).WithMany(p => p.VaccinationServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Vaccinati__categ__5BE2A6F2");
        });

        modelBuilder.Entity<VaccinationServiceVaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationServiceVaccinationId).HasName("PK__Vaccinat__79FC7EDCC0592D09");

            entity.ToTable("VaccinationService_Vaccination");

            entity.Property(e => e.VaccinationServiceVaccinationId).HasColumnName("vaccinationService_VaccinationId");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Service).WithMany(p => p.VaccinationServiceVaccinations)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Vaccinati__servi__5FB337D6");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationServiceVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Vaccinati__vacci__5EBF139D");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visit__4D3AA1BE6A454760");

            entity.ToTable("Visit");

            entity.Property(e => e.VisitId).HasColumnName("VisitID");
            entity.Property(e => e.AppointmentId).HasColumnName("appointmentID");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.VisitDate)
                .HasColumnType("datetime")
                .HasColumnName("visitDate");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Visits)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visit__appointme__75A278F5");
        });

        modelBuilder.Entity<VisitDayChangeRequest>(entity =>
        {
            entity.HasKey(e => e.ChangeRequestId).HasName("PK__VisitDay__34C330EC82F4828B");

            entity.ToTable("VisitDayChangeRequest");

            entity.Property(e => e.ChangeRequestId).HasColumnName("changeRequestId");
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.Reason)
                .HasMaxLength(1000)
                .HasColumnName("reason");
            entity.Property(e => e.RequestedDate)
                .HasColumnType("datetime")
                .HasColumnName("requestedDate");
            entity.Property(e => e.RequestedDateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("requestedDateAt");
            entity.Property(e => e.StaffNotes)
                .HasMaxLength(1000)
                .HasColumnName("staffNotes");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.VisitId).HasColumnName("VisitID");

            entity.HasOne(d => d.Patient).WithMany(p => p.VisitDayChangeRequests)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VisitDayC__patie__02084FDA");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitDayChangeRequests)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VisitDayC__Visit__01142BA1");
        });

        modelBuilder.Entity<VisitVaccination>(entity =>
        {
            entity.HasKey(e => e.AppointmentDoseVaccinationId).HasName("PK__Visit_Va__7C6C803CA436A6E4");

            entity.ToTable("Visit_Vaccination");

            entity.Property(e => e.AppointmentDoseVaccinationId).HasColumnName("appointmentDoseVaccinationID");
            entity.Property(e => e.AppointmentVaccinationId).HasColumnName("appointmentVaccinationID");
            entity.Property(e => e.VisitId).HasColumnName("VisitID");

            entity.HasOne(d => d.AppointmentVaccination).WithMany(p => p.VisitVaccinations)
                .HasForeignKey(d => d.AppointmentVaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visit_Vac__appoi__7D439ABD");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitVaccinations)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visit_Vac__Visit__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
