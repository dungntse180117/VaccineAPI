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

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentVaccination> AppointmentVaccinations { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<IntervalUnit> IntervalUnits { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

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
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267251E1DC31266");

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
                .HasConstraintName("FK__Account__roleId__4BAC3F29");
        });

        modelBuilder.Entity<AgeUnit>(entity =>
        {
            entity.HasKey(e => e.AgeUnitId).HasName("PK__AgeUnit__ABBA3B1805561861");

            entity.ToTable("AgeUnit");

            entity.Property(e => e.AgeUnitId).HasColumnName("ageUnitId");
            entity.Property(e => e.AgeUnitName)
                .HasMaxLength(10)
                .HasColumnName("ageUnitName");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__D067651EEED9681D");

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
                .HasConstraintName("FK__Appointme__confi__02FC7413");

            entity.HasOne(d => d.RegistrationDetail).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.RegistrationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__regis__02084FDA");
        });

        modelBuilder.Entity<AppointmentVaccination>(entity =>
        {
            entity.HasKey(e => e.AppointmentVaccinationId).HasName("PK__Appointm__F3AFE825F3EDD7E9");

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
                .HasConstraintName("FK__Appointme__appoi__08B54D69");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.AppointmentVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__vacci__09A971A2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D8917B06C6");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__Configur__3FEDA8E6B39A9CB6");

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
            entity.HasKey(e => e.DiseaseId).HasName("PK__Disease__49A1955731220AD3");

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
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC45A3D0F7F");

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
                .HasConstraintName("FK__Feedback__accoun__1AD3FDA4");

            entity.HasOne(d => d.VaccinationHistory).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.VaccinationHistoryId)
                .HasConstraintName("FK__Feedback__vaccin__19DFD96B");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__Image__C5BC81A61DF83539");

            entity.ToTable("Image");

            entity.Property(e => e.ImgId).HasColumnName("imgId");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .HasColumnName("img");
        });

        modelBuilder.Entity<IntervalUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Interval__55D7923559ABFC74");

            entity.ToTable("IntervalUnit");

            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.UnitName)
                .HasMaxLength(10)
                .HasColumnName("unitName");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__A17005ECC362291C");

            entity.ToTable("Patient");

            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
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

            entity.HasOne(d => d.Account).WithMany(p => p.Patients)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__account__5165187F");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId).HasName("PK__PaymentS__29CD0BBCC959E56B");

            entity.ToTable("PaymentStatus");

            entity.Property(e => e.PaymentStatusId)
                .ValueGeneratedNever()
                .HasColumnName("paymentStatusId");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__A3DB1415F03235A8");

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
                .HasConstraintName("FK__Registrat__accou__72C60C4A");

            entity.HasOne(d => d.Service).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Registrat__servi__73BA3083");
        });

        modelBuilder.Entity<RegistrationDetail>(entity =>
        {
            entity.HasKey(e => e.RegistrationDetailId).HasName("PK__Registra__466FE995A91693E1");

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
                .HasConstraintName("FK__Registrat__patie__7F2BE32F");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationDetails)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__7E37BEF6");
        });

        modelBuilder.Entity<RegistrationPatient>(entity =>
        {
            entity.HasKey(e => e.RegistrationPatientId).HasName("PK__Registra__335EAAC95134078F");

            entity.ToTable("Registration_Patient");

            entity.Property(e => e.RegistrationPatientId).HasColumnName("registrationPatientID");
            entity.Property(e => e.PatientId).HasColumnName("patientId");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");

            entity.HasOne(d => d.Patient).WithMany(p => p.RegistrationPatients)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__patie__7B5B524B");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationPatients)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__7A672E12");
        });

        modelBuilder.Entity<RegistrationVaccination>(entity =>
        {
            entity.HasKey(e => e.RegistrationPatientId).HasName("PK__Registra__335EAAC9D7F3A8BB");

            entity.ToTable("Registration_Vaccination");

            entity.Property(e => e.RegistrationPatientId).HasColumnName("registrationPatientID");
            entity.Property(e => e.RegistrationId).HasColumnName("registrationID");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Registration).WithMany(p => p.RegistrationVaccinations)
                .HasForeignKey(d => d.RegistrationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__regis__76969D2E");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.RegistrationVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Registrat__vacci__778AC167");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A0E9662C9");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<TrafficLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrafficL__3213E83FEF04F66A");

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
                .HasConstraintName("FK__TrafficLo__accou__4E88ABD4");
        });

        modelBuilder.Entity<Vaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationId).HasName("PK__Vaccinat__BF4AE609B45678C7");

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
                .HasConstraintName("FK__Vaccinati__ageUn__5DCAEF64");

            entity.HasOne(d => d.Unit).WithMany(p => p.Vaccinations)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Vaccinati__unitI__5EBF139D");
        });

        modelBuilder.Entity<VaccinationDisease>(entity =>
        {
            entity.HasKey(e => e.VaccinationDiseId).HasName("PK__Vaccinat__C7D778B0CD82DD19");

            entity.ToTable("Vaccination_Disease");

            entity.Property(e => e.VaccinationDiseId).HasColumnName("vaccinationDiseId");
            entity.Property(e => e.DiseaseId).HasColumnName("diseaseId");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Disease).WithMany(p => p.VaccinationDiseases)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__disea__6754599E");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationDiseases)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__66603565");
        });

        modelBuilder.Entity<VaccinationHistory>(entity =>
        {
            entity.HasKey(e => e.VaccinationHistoryId).HasName("PK__Vaccinat__3FD342CAEF6AF769");

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
                .HasConstraintName("FK__Vaccinati__patie__17036CC0");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__160F4887");

            entity.HasOne(d => d.Visit).WithMany(p => p.VaccinationHistories)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__Visit__151B244E");
        });

        modelBuilder.Entity<VaccinationImage>(entity =>
        {
            entity.HasKey(e => e.VaccinationImgId).HasName("PK__Vaccinat__058FD062B5524241");

            entity.ToTable("Vaccination_Image");

            entity.Property(e => e.VaccinationImgId).HasColumnName("vaccinationImgId");
            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.ImgId).HasColumnName("imgId");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Account).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__accou__6383C8BA");

            entity.HasOne(d => d.Img).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.ImgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__imgId__619B8048");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationImages)
                .HasForeignKey(d => d.VaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vaccinati__vacci__628FA481");
        });

        modelBuilder.Entity<VaccinationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Vaccinat__4550733F60F95863");

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
                .HasConstraintName("FK__Vaccinati__categ__6C190EBB");
        });

        modelBuilder.Entity<VaccinationServiceVaccination>(entity =>
        {
            entity.HasKey(e => e.VaccinationServiceVaccinationId).HasName("PK__Vaccinat__79FC7EDCD8350F0D");

            entity.ToTable("VaccinationService_Vaccination");

            entity.Property(e => e.VaccinationServiceVaccinationId).HasColumnName("vaccinationService_VaccinationId");
            entity.Property(e => e.ServiceId).HasColumnName("serviceID");
            entity.Property(e => e.VaccinationId).HasColumnName("vaccinationId");

            entity.HasOne(d => d.Service).WithMany(p => p.VaccinationServiceVaccinations)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Vaccinati__servi__6FE99F9F");

            entity.HasOne(d => d.Vaccination).WithMany(p => p.VaccinationServiceVaccinations)
                .HasForeignKey(d => d.VaccinationId)
                .HasConstraintName("FK__Vaccinati__vacci__6EF57B66");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visit__4D3AA1BE4EBFEE74");

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
                .HasConstraintName("FK__Visit__appointme__05D8E0BE");
        });

        modelBuilder.Entity<VisitDayChangeRequest>(entity =>
        {
            entity.HasKey(e => e.ChangeRequestId).HasName("PK__VisitDay__34C330ECE7EA45C6");

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
                .HasConstraintName("FK__VisitDayC__patie__123EB7A3");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitDayChangeRequests)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VisitDayC__Visit__114A936A");
        });

        modelBuilder.Entity<VisitVaccination>(entity =>
        {
            entity.HasKey(e => e.AppointmentDoseVaccinationId).HasName("PK__Visit_Va__7C6C803CA66EC910");

            entity.ToTable("Visit_Vaccination");

            entity.Property(e => e.AppointmentDoseVaccinationId).HasColumnName("appointmentDoseVaccinationID");
            entity.Property(e => e.AppointmentVaccinationId).HasColumnName("appointmentVaccinationID");
            entity.Property(e => e.VisitId).HasColumnName("VisitID");

            entity.HasOne(d => d.AppointmentVaccination).WithMany(p => p.VisitVaccinations)
                .HasForeignKey(d => d.AppointmentVaccinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visit_Vac__appoi__0D7A0286");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitVaccinations)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visit_Vac__Visit__0C85DE4D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
