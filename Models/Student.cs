using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? StuFirstName { get; set; }

    public string? StuLastName { get; set; }

    public long? StuDoB { get; set; }

    public virtual ICollection<EnrollmentList> EnrollmentLists { get; set; } = new List<EnrollmentList>();
}
