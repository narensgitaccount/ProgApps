using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class User
{
    public int UserID { get; set; }

    public string EmailAddress { get; set; } = null!;

    public int Question1Selection { get; set; }

    public string Question1Response { get; set; } = null!;

    public int Question2Selection { get; set; }

    public string Question2Response { get; set; } = null!;

    public int? RetrievalFailureCount { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public bool IsConfirmed { get; set; }

    public virtual ICollection<PrivateSchoolApplication> PrivateSchoolApplications { get; set; } = new List<PrivateSchoolApplication>();

    public virtual SecurityQuestionList Question1SelectionNavigation { get; set; } = null!;

    public virtual SecurityQuestionList Question2SelectionNavigation { get; set; } = null!;
}
