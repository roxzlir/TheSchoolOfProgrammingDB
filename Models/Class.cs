using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string? ClassName { get; set; }

    public virtual ICollection<EnrollmentList> EnrollmentLists { get; set; } = new List<EnrollmentList>();
}
