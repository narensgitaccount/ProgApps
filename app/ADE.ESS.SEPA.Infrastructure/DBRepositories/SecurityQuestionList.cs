using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class SecurityQuestionList
{
    public int QuestionID { get; set; }

    public int QuestionNumber { get; set; }

    public int SeqNumber { get; set; }

    public string QuestionText { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public virtual ICollection<User> UserQuestion1SelectionNavigations { get; set; } = new List<User>();

    public virtual ICollection<User> UserQuestion2SelectionNavigations { get; set; } = new List<User>();
}
