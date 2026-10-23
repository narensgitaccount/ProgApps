using System;
using System.Collections.Generic;

namespace ADE.ESS.SEPA.Infrastructure.DBRepositories;

public partial class TeachersStaffTraining
{
    public int TeacherCourseID { get; set; }

    public int TeacherID { get; set; }

    public string CourseName { get; set; } = null!;

    public double TrainingHours { get; set; }

    public DateTime CreatedDate { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime LastModifiedDate { get; set; }

    public virtual TeachersStaff Teacher { get; set; } = null!;
}
