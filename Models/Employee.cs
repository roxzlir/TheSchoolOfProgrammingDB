using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmpFirstName { get; set; }

    public string? EmpLastName { get; set; }

    public long? EmpDoB { get; set; }

    public DateTime? HiredDate { get; set; }

    public int? FkProfessionId { get; set; }

    public decimal? Salery { get; set; }

    public virtual ICollection<EnrollmentList> EnrollmentLists { get; set; } = new List<EnrollmentList>();

    public virtual Profession? FkProfession { get; set; }
}
