using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Profession
{
    public int ProfessionId { get; set; }

    public string? ProTitle { get; set; }

    public int? FkDepartmentId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Department? FkDepartment { get; set; }
}
