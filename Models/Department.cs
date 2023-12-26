using System;
using System.Collections.Generic;

namespace TheSchoolOfProgrammingDB.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
