using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class ADEContact
{
    public int AdeContactId { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Status { get; set; }

    public string Email { get; set; } = null!;

    public string? MailingAddress { get; set; }

    public string? OtherDetails { get; set; }

    public DateTime CreateDate { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public int DisplayOrder { get; set; }
}
