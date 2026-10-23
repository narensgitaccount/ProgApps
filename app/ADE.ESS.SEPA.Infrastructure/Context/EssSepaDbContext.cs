using System;
using System.Collections.Generic;
using ADE.ESS.SEPA.Infrastructure.DBRepositories;
using Microsoft.EntityFrameworkCore;

namespace ADE.ESS.SEPA.Infrastructure.Context;

public partial class EssSepaDbContext : DbContext
{
    public EssSepaDbContext()
    {
    }

    public EssSepaDbContext(DbContextOptions<EssSepaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ADEContact> ADEContacts { get; set; }

    public virtual DbSet<ApplicationHistory> ApplicationHistories { get; set; }

    public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }

    public virtual DbSet<ChangeHistory> ChangeHistories { get; set; }

    public virtual DbSet<EDPApplication> EDPApplications { get; set; }

    public virtual DbSet<EmailDeliveryLog> EmailDeliveryLogs { get; set; }

    public virtual DbSet<FiscalYearColumnSetting> FiscalYearColumnSettings { get; set; }

    public virtual DbSet<FiscalYearShowHideColumn> FiscalYearShowHideColumns { get; set; }

    public virtual DbSet<FiscalYearThreshold> FiscalYearThresholds { get; set; }

    public virtual DbSet<FiscalYearsLookup> FiscalYearsLookups { get; set; }

    public virtual DbSet<LogTable> LogTables { get; set; }

    public virtual DbSet<LookupGrade> LookupGrades { get; set; }

    public virtual DbSet<LookupManagementValue> LookupManagementValues { get; set; }

    public virtual DbSet<LookupNeed> LookupNeeds { get; set; }

    public virtual DbSet<ManagementReport> ManagementReports { get; set; }

    public virtual DbSet<PDSApplicationSite> PDSApplicationSites { get; set; }

    public virtual DbSet<PDSAttachment> PDSAttachments { get; set; }

    public virtual DbSet<PDSSiteClassroom> PDSSiteClassrooms { get; set; }

    public virtual DbSet<PDSSiteGrade> PDSSiteGrades { get; set; }

    public virtual DbSet<PDSSiteNeed> PDSSiteNeeds { get; set; }

    public virtual DbSet<PDSSiteTeacher> PDSSiteTeachers { get; set; }

    public virtual DbSet<PrivateSchoolApplication> PrivateSchoolApplications { get; set; }

    public virtual DbSet<SecurityQuestionList> SecurityQuestionLists { get; set; }

    public virtual DbSet<SiteAttachment> SiteAttachments { get; set; }

    public virtual DbSet<StaffCertificationName> StaffCertificationNames { get; set; }

    public virtual DbSet<StateAbbreviation> StateAbbreviations { get; set; }

    public virtual DbSet<StatusType> StatusTypes { get; set; }

    public virtual DbSet<TeachersStaff> TeachersStaffs { get; set; }

    public virtual DbSet<TeachersStaffAttachment> TeachersStaffAttachments { get; set; }

    public virtual DbSet<TeachersStaffTraining> TeachersStaffTrainings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwSchool> VwSchools { get; set; }

    public virtual DbSet<vwEdOrg> vwEdOrgs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ESSPrivateEDPEntities");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ADEContact>(entity =>
        {
            entity.HasKey(e => e.AdeContactId).HasName("PK_dbo.ADEContact");

            entity.ToTable("ADEContact");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.MailingAddress).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OtherDetails).HasMaxLength(1000);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicationHistory>(entity =>
        {
            entity.HasKey(e => e.LogID);

            entity.ToTable("ApplicationHistory");

            entity.Property(e => e.ApplicationType).HasMaxLength(100);
            entity.Property(e => e.ChangeType).HasMaxLength(100);
            entity.Property(e => e.ChangedBy).HasMaxLength(100);
            entity.Property(e => e.LogDtm).HasColumnType("datetime");
        });

        modelBuilder.Entity<AttachmentType>(entity =>
        {
            entity.HasKey(e => e.AttachmentTypeID).HasName("PK__Attachme__5C63AB440D94FC20");

            entity.ToTable("AttachmentType");

            entity.Property(e => e.AttachmentTypeID).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChangeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07B24DC543");

            entity.ToTable("ChangeHistory");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EDPApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK_Application");

            entity.ToTable("EDPApplication", tb => tb.HasTrigger("UpdateEntityName"));

            entity.Property(e => e.AgeRange)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CaseManagement)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ContinuumOfService)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EDPComponent)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EntityName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LastCertifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.MedicationDispensing)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.MentalHealth)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.OutsideAgency)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.PointOfContactEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactFax)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactFirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactLastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactTitle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionBehavioralPlan)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionCompDataCollection)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionCompEdEligibility)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionCompExitCriteria)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionCompGoalProgress)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionCompPlacementChanges)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionEdpPlacement)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionExitCriteria)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionFitWithContinuum)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.QuestionRestrictiveEnvironment)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Separation)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.SignatureDate).HasColumnType("datetime");
            entity.Property(e => e.SignedBy).IsUnicode(false);
            entity.Property(e => e.SignerTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StatementAssurance).IsUnicode(false);
            entity.Property(e => e.StudentCharacter)
                .HasMaxLength(8000)
                .IsUnicode(false);

            entity.HasOne(d => d.FiscalYearNavigation).WithMany(p => p.EDPApplications)
                .HasForeignKey(d => d.FiscalYear)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EDPApplication_FiscalYears");

            entity.HasOne(d => d.Status).WithMany(p => p.EDPApplications)
                .HasForeignKey(d => d.StatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EDPApplication_StatusTypes");
        });

        modelBuilder.Entity<EmailDeliveryLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__EmailDel__5E5486489BF7B2F6");

            entity.ToTable("EmailDeliveryLog");

            entity.Property(e => e.ApplicationName).HasMaxLength(256);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ToAddress).HasMaxLength(500);
            entity.Property(e => e.TriggeredBy).HasMaxLength(256);
        });

        modelBuilder.Entity<FiscalYearColumnSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FiscalYe__3214EC07DC6DF947");

            entity.Property(e => e.ColumnName).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ModifyUserName).HasMaxLength(100);
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<FiscalYearShowHideColumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FiscalYe__3214EC074F4A5C5F");

            entity.Property(e => e.ApplicationType).HasMaxLength(50);
            entity.Property(e => e.ColumnName).HasMaxLength(50);

            entity.HasOne(d => d.FicsalYearThreshold).WithMany(p => p.FiscalYearShowHideColumns)
                .HasForeignKey(d => d.FicsalYearThresholdId)
                .HasConstraintName("FK__FiscalYea__Ficsa__21D600EE");
        });

        modelBuilder.Entity<FiscalYearThreshold>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FiscalYe__3214EC07BEE0ED4A");

            entity.ToTable("FiscalYearThreshold");
        });

        modelBuilder.Entity<FiscalYearsLookup>(entity =>
        {
            entity.HasKey(e => e.FiscalYear).HasName("PK_FiscalYears");

            entity.ToTable("FiscalYearsLookup");

            entity.Property(e => e.FiscalYear).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LogTable>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_AltLogTable");

            entity.ToTable("LogTable");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Exception).HasColumnType("text");
            entity.Property(e => e.LogLevel).HasColumnType("text");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.Origin)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StackTrace).HasColumnType("text");
        });

        modelBuilder.Entity<LookupGrade>(entity =>
        {
            entity.HasKey(e => e.GradeID);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.GradeClass)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.GradeName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.GradeShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<LookupManagementValue>(entity =>
        {
            entity.HasKey(e => e.LookupName);

            entity.Property(e => e.LookupName).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LookUpManagementValueId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<LookupNeed>(entity =>
        {
            entity.HasKey(e => e.NeedID);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayClass)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NeedCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NeedDescription)
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ManagementReport>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF_ManagementReports_CreatedDate")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReportDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ReportDisplayName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ReportName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())", "DF_ManagementReports_UpdatedDate")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PDSApplicationSite>(entity =>
        {
            entity.HasKey(e => e.SiteID);

            entity.HasIndex(e => new { e.ApplicationID, e.EDPApplicationID, e.SiteName }, "IXNCPDSApplicationSites_AppID_EDPID");

            entity.Property(e => e.AgeRange)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CaseManagement)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactEmailAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ContinuumOfService)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CounsellingServices)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)", "DF_PDSApplicationSites_CounsellingServices");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DhsLicenseNumberAtSite)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DistrictLocation)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.EDPBuildingLocation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EDPClassRoomArea)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EDPComponent)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPContactDetails)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPEntitySiteType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EDPSeclusionArea)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPServices)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPStudentTransferNum)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPSupervision)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EDPSupervisionLevel)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.MedicationDispensing)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.MentalHealth)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.OutsideAgency)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ProgramName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RecordStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.SeclusionRooms)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)", "DF_PDSApplicationSites_SeclusionRooms");
            entity.Property(e => e.Separation)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.SiteName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StudentCharacter)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PDSAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentID);

            entity.Property(e => e.AttachmentName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AttachmentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Not Specified");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PDSSiteClassroom>(entity =>
        {
            entity.HasKey(e => e.SiteClassroomID);

            entity.ToTable("PDSSiteClassroom");

            entity.Property(e => e.AgeRangeDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClassRoomName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Site).WithMany(p => p.PDSSiteClassrooms)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteClassroom_PDSApplicationSites");
        });

        modelBuilder.Entity<PDSSiteGrade>(entity =>
        {
            entity.HasKey(e => e.SiteGradeID);

            entity.ToTable("PDSSiteGrade");

            entity.HasIndex(e => new { e.SiteID, e.GradeID }, "IXNCPDSSiteGrade_SiteID_GradeID");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Grade).WithMany(p => p.PDSSiteGrades)
                .HasForeignKey(d => d.GradeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteGrade_LookupGrades");

            entity.HasOne(d => d.Site).WithMany(p => p.PDSSiteGrades)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteGrade_PDSApplicationSites");
        });

        modelBuilder.Entity<PDSSiteNeed>(entity =>
        {
            entity.HasKey(e => e.SiteNeedID);

            entity.HasIndex(e => new { e.SiteID, e.NeedID }, "IXNCPDSSiteNeeds_SiteID_NeedID");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Need).WithMany(p => p.PDSSiteNeeds)
                .HasForeignKey(d => d.NeedID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteNeeds_LookupNeeds");

            entity.HasOne(d => d.Site).WithMany(p => p.PDSSiteNeeds)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteNeeds_PDSApplicationSites");
        });

        modelBuilder.Entity<PDSSiteTeacher>(entity =>
        {
            entity.HasKey(e => e.SiteTeacherID);

            entity.ToTable("PDSSiteTeacher");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PDSSiteStartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Site).WithMany(p => p.PDSSiteTeachers)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteTeacher_PDSApplicationSites");

            entity.HasOne(d => d.Teacher).WithMany(p => p.PDSSiteTeachers)
                .HasForeignKey(d => d.TeacherID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDSSiteTeacher_TeachersStaff");
        });

        modelBuilder.Entity<PrivateSchoolApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationID);

            entity.ToTable("PrivateSchoolApplication");

            entity.HasIndex(e => new { e.FiscalYear, e.ApplicationID }, "IXNCPrivateSchoolApplication_FY_AppID");

            entity.Property(e => e.CorporateContactEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactFax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactFirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactLastName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorporateContactTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateMailingAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateMailingAddress2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CorporateMailingCity)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CorporateMailingPostalCode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CorporateName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CrisisInterventionEmailAddress).HasMaxLength(100);
            entity.Property(e => e.CrisisInterventionFax).HasMaxLength(100);
            entity.Property(e => e.CrisisInterventionFirstName).HasMaxLength(200);
            entity.Property(e => e.CrisisInterventionLastName).HasMaxLength(200);
            entity.Property(e => e.CrisisInterventionPhoneNumber).HasMaxLength(100);
            entity.Property(e => e.CrisisInterventionResponsiblePerson).HasDefaultValue(0);
            entity.Property(e => e.CrisisInterventionTitle).HasMaxLength(500);
            entity.Property(e => e.CurriculumLink)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("", "DF__PrivateSc__Curri__382F5661");
            entity.Property(e => e.DhsLicenseNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_PrivateSchoolApplication_DhsLicenseNumber");
            entity.Property(e => e.EDPStatementAssurance).IsUnicode(false);
            entity.Property(e => e.ExperienceComments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.FCCCardNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsStudentDemo3to5).HasDefaultValue(0);
            entity.Property(e => e.IsStudentDemoKto12).HasDefaultValue(0);
            entity.Property(e => e.IsStudentDemoMET).HasDefaultValue(0);
            entity.Property(e => e.LastCertifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.PDSStatementAssurance).IsUnicode(false);
            entity.Property(e => e.PointOfContactEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PointOfContactFax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PointOfContactFirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactLastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PointOfContactName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PointOfContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PointOfContactTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PolicyLink)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("", "DF__PrivateSc__Polic__36470DEF");
            entity.Property(e => e.ProgramInfoBehavioralData)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ProgramInfoCollaborationProcess)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ProgramInfoExitCriteria)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ProgramInfoIntakeProcess)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ResidentialSchoolStatementAssurance).IsUnicode(false);
            entity.Property(e => e.RtcWithEdpStatementAssurance).IsUnicode(false);
            entity.Property(e => e.SchoolAdminContactEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SchoolAdminContactFax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SchoolAdminContactFirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(".", "DF__tmp_ms_xx__Schoo__76969D2E");
            entity.Property(e => e.SchoolAdminContactLastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("", "DF_PrivateSchoolApplication_SchoolAdminContactLastName");
            entity.Property(e => e.SchoolAdminContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SchoolAdminContactTitle)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SignDate).HasColumnType("datetime");
            entity.Property(e => e.SignedBy).IsUnicode(false);
            entity.Property(e => e.SignerTitle)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(1, "DF__PrivateSc__Statu__41B8C09B");

            entity.HasOne(d => d.CorporateMailingState).WithMany(p => p.PrivateSchoolApplications)
                .HasForeignKey(d => d.CorporateMailingStateId)
                .HasConstraintName("FK_PrivateSchoolApplication_StateId");

            entity.HasOne(d => d.FiscalYearNavigation).WithMany(p => p.PrivateSchoolApplications)
                .HasForeignKey(d => d.FiscalYear)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrivateSchoolApplication_FiscalYears");

            entity.HasOne(d => d.User).WithMany(p => p.PrivateSchoolApplications)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrivateSchoolApplication_User");
        });

        modelBuilder.Entity<SecurityQuestionList>(entity =>
        {
            entity.HasKey(e => e.QuestionID);

            entity.ToTable("SecurityQuestionList");

            entity.Property(e => e.QuestionID).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QuestionText).HasMaxLength(1000);
        });

        modelBuilder.Entity<SiteAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentID);

            entity.ToTable("SiteAttachment");

            entity.Property(e => e.Filename)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("datetime");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Site).WithMany(p => p.SiteAttachments)
                .HasForeignKey(d => d.SiteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SiteAttachment_PDSApplicationSites");
        });

        modelBuilder.Entity<StaffCertificationName>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StateAbbreviation>(entity =>
        {
            entity.ToTable("StateAbbreviation");

            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ShortDescription).HasMaxLength(20);
        });

        modelBuilder.Entity<StatusType>(entity =>
        {
            entity.HasKey(e => e.StatusID);

            entity.Property(e => e.StatusID).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TeachersStaff>(entity =>
        {
            entity.HasKey(e => e.TeacherID);

            entity.ToTable("TeachersStaff");

            entity.HasIndex(e => new { e.ApplicationID, e.EDPApplicationID, e.LastName, e.FirstName }, "IXNCTeachersStaff_AppID_EDPID");

            entity.Property(e => e.AffectedSiteIDs).IsUnicode(false);
            entity.Property(e => e.AffectedSiteNames).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FCCCardExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.FCCCardStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FccCardNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LicenseFilename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("", "DF__TeachersS__Licen__4589517F");
            entity.Property(e => e.RecordStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.StaffTypeOther)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeachersStaffAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentID);

            entity.ToTable("TeachersStaffAttachment");

            entity.Property(e => e.Filename)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UploadDate).HasColumnType("datetime");
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeachersStaffAttachments)
                .HasForeignKey(d => d.TeacherID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeachersStaffAttachment_TeachersStaff");
        });

        modelBuilder.Entity<TeachersStaffTraining>(entity =>
        {
            entity.HasKey(e => e.TeacherCourseID);

            entity.ToTable("TeachersStaffTraining");

            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastModifiedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeachersStaffTrainings)
                .HasForeignKey(d => d.TeacherID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeachersStaffTraining_TeachersStaff");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK_UserSecurity");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())", "DF__tmp_ms_xx__Creat__6CD828CA")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(150);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedDate)
                .HasDefaultValueSql("(getdate())", "DF__tmp_ms_xx__LastM__6DCC4D03")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Question1SelectionNavigation).WithMany(p => p.UserQuestion1SelectionNavigations)
                .HasForeignKey(d => d.Question1Selection)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSecurity_Question1");

            entity.HasOne(d => d.Question2SelectionNavigation).WithMany(p => p.UserQuestion2SelectionNavigations)
                .HasForeignKey(d => d.Question2Selection)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSecurity_Question2");
        });

        modelBuilder.Entity<VwSchool>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwSchools");

            entity.Property(e => e.DistrictCTDS).HasMaxLength(12);
            entity.Property(e => e.DistrictName).HasMaxLength(100);
            entity.Property(e => e.SchoolCTDS).HasMaxLength(12);
            entity.Property(e => e.SchoolName).HasMaxLength(100);
        });

        modelBuilder.Entity<vwEdOrg>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwEdOrg");

            entity.Property(e => e.CTDS).HasMaxLength(12);
            entity.Property(e => e.NameOfInstitution).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
