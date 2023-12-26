using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? SubjectName { get; set; }

    public string? CourseStatus { get; set; }

    public virtual ICollection<EnrollmentList> EnrollmentLists { get; set; } = new List<EnrollmentList>();
}
