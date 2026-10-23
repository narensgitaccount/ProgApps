using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class EDPApplication
{
    public int ApplicationId { get; set; }

    public int EntityId { get; set; }

    public string? EntityName { get; set; }

    public int StatusID { get; set; }

    public string StudentCharacter { get; set; } = null!;

    public string ContinuumOfService { get; set; } = null!;

    public string Separation { get; set; } = null!;

    public string MentalHealth { get; set; } = null!;

    public string CaseManagement { get; set; } = null!;

    public string MedicationDispensing { get; set; } = null!;

    public string OutsideAgency { get; set; } = null!;

    public string AgeRange { get; set; } = null!;

    public string EDPComponent { get; set; } = null!;

    public string? SignedBy { get; set; }

    public string? SignerTitle { get; set; }

    public DateTime? SignatureDate { get; set; }

    public string? StatementAssurance { get; set; }

    public int FiscalYear { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime? LastCertifiedDate { get; set; }

    public int? PriorApplicationID { get; set; }

    public bool HasBeenApproved { get; set; }

    public string PointOfContactFirstName { get; set; } = null!;

    public string PointOfContactLastName { get; set; } = null!;

    public string PointOfContactTitle { get; set; } = null!;

    public string PointOfContactPhone { get; set; } = null!;

    public string PointOfContactEmail { get; set; } = null!;

    public string? PointOfContactFax { get; set; }

    public string QuestionRestrictiveEnvironment { get; set; } = null!;

    public string QuestionEdpPlacement { get; set; } = null!;

    public string QuestionFitWithContinuum { get; set; } = null!;

    public string QuestionExitCriteria { get; set; } = null!;

    public string QuestionBehavioralPlan { get; set; } = null!;

    public string QuestionCompDataCollection { get; set; } = null!;

    public string QuestionCompGoalProgress { get; set; } = null!;

    public string QuestionCompPlacementChanges { get; set; } = null!;

    public string QuestionCompExitCriteria { get; set; } = null!;

    public string QuestionCompEdEligibility { get; set; } = null!;

    public int? ApplicationKey { get; set; }

    public bool? IsCurrent { get; set; }

    public virtual FiscalYearsLookup FiscalYearNavigation { get; set; } = null!;

    public virtual StatusType Status { get; set; } = null!;
}
