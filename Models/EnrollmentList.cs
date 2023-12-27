using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class EnrollmentList
{
    public int? FkStudentId { get; set; }

    public int? FkCourseId { get; set; }

    public int? FkEmployeeId { get; set; }

    public int? FkClassId { get; set; }

    public int EnrollmentId { get; set; }

    public int? Grade { get; set; }

    public DateTime? GradeDate { get; set; }

    public virtual Class? FkClass { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Employee? FkEmployee { get; set; }

    public virtual Student? FkStudent { get; set; }
}
