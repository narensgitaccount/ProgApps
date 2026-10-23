using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class AttachmentType
{
    public int AttachmentTypeID { get; set; }

    public string Description { get; set; } = null!;

    public int OrderID { get; set; }
}
