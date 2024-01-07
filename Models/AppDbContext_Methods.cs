using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TheSchoolOfProgrammingDB.Models;

public partial class AppDbContext_Methods : DbContext
{
    public static void Run()
    {
        int userMenuChoice;
        do
        {
            Console.Clear();
            Logo();
            Console.WriteLine(" - Welcome to The School Of Programming Database - ");
            Console.WriteLine("(1) - Full employee details list");
            Console.WriteLine("(2) - Department info");
            Console.WriteLine("(3) - Student info");
            Console.WriteLine("(4) - Course info");
            Console.WriteLine("(5) - Add new Student");
            Console.WriteLine("(6) - Add new Employee");
            Console.WriteLine("(7) - Enroll student in course");
            Console.WriteLine("(8) - Add/Change grade");
            Console.WriteLine("(0) - Exit");
            Console.Write("Please select from the menu above: ");
            userMenuChoice = Program.GetUserInput();
            switch (userMenuChoice)
            {
                case 1:
                    Console.Clear();
                    PrintEmployees();
                    break;
                case 2:
                    int i;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("- Department info -");
                        Console.WriteLine("(1) - Employees in every department");
                        Console.WriteLine("(2) - Total monthly salery expens/department");
                        Console.WriteLine("(3) - Averege monthly salery expens/department");
                        Console.WriteLine("(0) - Exit");
                        i = GetUserInput();
                        Console.Clear();
                        switch (i)
                        {
                            case 1:
 
                                PrintEmpDepartments();
                                break;
                            case 2:
                                PrintDepSalerys();
                                break;
                            case 3:
                                PrintDepAvSalery();
                                break;
                        }
                        if (i == 0)
                        {
                            break;
                        }
                    }
                    break;
                case 3:
                    int y;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("- Student info -");
                        Console.WriteLine("(1) - Print all students");
                        Console.WriteLine("(2) - Print student class list");
                        Console.WriteLine("(3) - Student ID search");
                        Console.WriteLine("(0) - Exit");
                        y = GetUserInput();
                        Console.Clear();
                        switch (y)
                        {
                            case 1:
                                PrintAllStudents();
                                break;
                            case 2:
                                PrintStudentsInClasses();
                                break;
                            case 3:
                                GetStuInfo();
                                break;
                        }
                        if (y == 0)
                        {
                            break;
                        }
                    }
                    break;
                case 4:
                    int j;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("- Course info -");
                        Console.WriteLine("(1) - Print all available courses");
                        Console.WriteLine("(2) - Print all unavailable courses");
                        Console.WriteLine("(0) - Exit");
                        j = GetUserInput();
                        Console.Clear();
                        switch (j)
                        {
                            case 1:
                                PrintACourses();
                                break;
                            case 2:
                                PrintNaCourses();
                                break;
                        }
                        if (j == 0)
                        {
                            break;
                        }
                    }
                    break;
                case 5:
                    Console.WriteLine("- Add a new student to database -");
                    AddStudent();
                    break;
                case 6:
                    AddEmployee();
                    break;
                case 7:
                    EnrollStudent();
                    break;
                case 8:
                    int m;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("- Grades -");
                        Console.WriteLine("(1) - Set first time grade");
                        Console.WriteLine("(2) - Change existing grade");
                        Console.WriteLine("(0) - Exit");
                        m = GetUserInput();
                        Console.Clear();
                        switch (m)
                        {
                            case 1:
                                SetGrade();
                                break;
                            case 2:
                                ChangeGrade();
                                break;
                        }
                        if (m == 0)
                        {
                            break;
                        }
                    }
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        } while (userMenuChoice != 0);
    }
    public static void GetStuInfo()
    {
        AppDbContext dbContext = new AppDbContext();
        var studentIdList = dbContext.Students.ToList();
        while (true)
        {
            Console.WriteLine("(999) - StudentID list");
            Console.WriteLine("(990) - Exit to main menu");
            Console.Write("Please enter a Student ID for student info: ");
            int studentID = GetUserInput(); //Tar in ett id från användaren

            var studentInfo = dbContext.GetStudentInfo(studentID).SingleOrDefault(); //Kör den mot GetStudentInfo som kör mot min Stored Procedure

            if (studentInfo != null)
            {
                Console.Clear();
                Console.WriteLine($"Student Name: {studentInfo.StuFirstName} {studentInfo.StuLastName}");
                Console.WriteLine($"Date of Birth: {studentInfo.StuDoB}");
                Console.Write("Press any key when ready: ");
                Console.ReadKey();
            }
            else if (studentID == 990)
            {
                break;
            }
            else if (studentID == 999)
            {
                foreach (var id in studentIdList.OrderBy(i =>i.StudentId))
                {
                    Console.WriteLine($"ID: {id.StudentId}");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Student not found.");
                Console.Write("Press any key when ready: ");
                Console.ReadKey();
            }
        }

    }
    public static void PrintEmployees()
    {
        AppDbContext dbContext = new AppDbContext();
        var empInfo = from employee in dbContext.Employees //Här skapar tar jag inte både från Employee och från Profession för att kunna få fram korrekt data
                      join profession in dbContext.Professions on employee.FkProfessionId equals profession.ProfessionId
                      orderby profession.ProfessionId
                      select new
                      {
                          eFirstName = employee.EmpFirstName,
                          eLastName = employee.EmpLastName,
                          proName = profession.ProTitle,
                          yearsHired = EF.Functions.DateDiffYear(employee.HiredDate, DateTime.Now), //Gör detta för att få ut antal år istället för bara datum
                          dateHired = employee.HiredDate
                      };



        foreach (var employee in empInfo)
        {
            DateTime date = Convert.ToDateTime(employee.dateHired); //Ville få bort dem tomma klockslagen från utskriften så sparar ner alla datum från employee.dateHired till
            string dateOnly = date.ToString("yyyy-MM-dd");  //date och gör sedan om dem till en string med formatet jag önskar.
            Console.WriteLine($"Employee: {employee.eFirstName} {employee.eLastName}\nWork title: {employee.proName}\nYears hired: {employee.yearsHired}\n" +
                $"(employed since: {dateOnly})\n-----------------------");
        }
        Console.WriteLine("Press any key when done: ");
        Console.ReadKey();
    } //Metod för att skriva ut alla employee's samt deras profession title
    public static void PrintEmpDepartments() //Här skriver jag ut antal employees/department samt kan man se alla employees mer detaljerat i varje department
    {
        AppDbContext dbContext = new AppDbContext();
        var empDepInfo = from employee in dbContext.Employees
                      join profession in dbContext.Professions on employee.FkProfessionId equals profession.ProfessionId
                      join departments in dbContext.Departments on profession.FkDepartmentId equals departments.DepartmentId
                      select new
                      {
                          depName = departments.DepartmentName,
                          depID = departments.DepartmentId,
                          eFirstName = employee.EmpFirstName,
                          eLastName = employee.EmpLastName,
                          hiredSince = employee.HiredDate,
                          profName = profession.ProTitle
                      };

        while (true)
        {
            Console.Clear();
            Console.WriteLine("(1) - Admin Team");
            Console.WriteLine($"Current employees: {empDepInfo.Where(x => x.depID == 2).Count()}");
            Console.WriteLine("(2) - Education Team");
            Console.WriteLine($"Current employees: {empDepInfo.Where(x => x.depID == 1).Count()}");
            Console.WriteLine("(3) - Management");
            Console.WriteLine($"Current employees: {empDepInfo.Where(x => x.depID == 3).Count()}");
            Console.WriteLine("(0) - Exit menu");
            Console.Write("For a full employee list of each department, press the department number or chose exit: ");
            int choice = GetUserInput();
            switch (choice)
            {
                case 1:
                    Console.Clear();
                    foreach (var emp in empDepInfo.Where(d => d.depID == 2))
                    {
                        DateTime date = Convert.ToDateTime(emp.hiredSince); //Ville få bort dem tomma klockslagen från utskriften så sparar ner alla datum från employee.dateHired till
                        string dateOnly = date.ToString("yyyy-MM-dd");  //date och gör sedan om dem till en string med formatet jag önskar.
                        Console.WriteLine($"Name: {emp.eLastName}, {emp.eFirstName}\nWork as: {emp.profName}\nHired since: {dateOnly}");
                        Console.WriteLine("-----------");
                    }
                    Console.Write("Press any key when ready: ");
                    Console.ReadKey();
                    break;
                case 2:
                    Console.Clear();
                    foreach (var emp in empDepInfo.Where(d => d.depID == 1))
                    {
                        DateTime date = Convert.ToDateTime(emp.hiredSince); //Ville få bort dem tomma klockslagen från utskriften så sparar ner alla datum från employee.dateHired till
                        string dateOnly = date.ToString("yyyy-MM-dd");  //date och gör sedan om dem till en string med formatet jag önskar.
                        Console.WriteLine($"Name: {emp.eLastName}, {emp.eFirstName}\nWork as: {emp.profName}\nHired since: {dateOnly}");
                        Console.WriteLine("-----------");
                    }
                    Console.Write("Press any key when ready: ");
                    Console.ReadKey();
                    break;
                case 3:
                    Console.Clear();
                    foreach (var emp in empDepInfo.Where(d => d.depID == 3))
                    {
                        DateTime date = Convert.ToDateTime(emp.hiredSince); //Ville få bort dem tomma klockslagen från utskriften så sparar ner alla datum från employee.dateHired till
                        string dateOnly = date.ToString("yyyy-MM-dd");  //date och gör sedan om dem till en string med formatet jag önskar.
                        Console.WriteLine($"Name: {emp.eLastName}, {emp.eFirstName}\nWork as: {emp.profName}\nHired since: {dateOnly}");
                        Console.WriteLine("-----------");
                    }
                    Console.Write("Press any key when ready: ");
                    Console.ReadKey();
                    break;
                default:
                    break;
            }
            if (choice == 0)
            {
                break;
            }
        }


    }
    public static void PrintDepSalerys()
    {
        AppDbContext dbContext = new AppDbContext();
        var empDepInfo = from employee in dbContext.Employees
                         join profession in dbContext.Professions on employee.FkProfessionId equals profession.ProfessionId
                         join departments in dbContext.Departments on profession.FkDepartmentId equals departments.DepartmentId
                         select new
                         {
                             depID = departments.DepartmentId,
                             eSalery = employee.Salery,
                         };

        Console.Clear();
        Console.WriteLine(" -- Admin Team --");
        Console.WriteLine($"Total salery for Admin department: {empDepInfo.Where(d => d.depID == 2).Sum(s => s.eSalery)} SEK/month");
        Console.WriteLine("\n -- Education Team --");
        Console.WriteLine($"Total salery for Education department: {empDepInfo.Where(d => d.depID == 1).Sum(s => s.eSalery)} SEK/month");
        Console.WriteLine("\n -- Management --");
        Console.WriteLine($"Total salery for Management department: {empDepInfo.Where(d => d.depID == 3).Sum(s => s.eSalery)} SEK/month");
        Console.Write("Press any key when ready: ");
        Console.ReadKey();
    }
    public static void PrintDepAvSalery()
    {
        AppDbContext dbContext = new AppDbContext();
        var empDepInfo = from employee in dbContext.Employees
                         join profession in dbContext.Professions on employee.FkProfessionId equals profession.ProfessionId
                         join departments in dbContext.Departments on profession.FkDepartmentId equals departments.DepartmentId
                         select new
                         {
                             depName = departments.DepartmentName,
                             eSalery = employee.Salery,
                         };

        var avgSaleryByDepartment = empDepInfo.GroupBy(d => d.depName)
                                             .Select(group => new
                                             {
                                                 Department = group.Key,
                                                 AverageSalery = group.Average(item => item.eSalery)
                                             });
        Console.Clear();
        foreach (var department in avgSaleryByDepartment)
        {
            Console.WriteLine($"Average salary for employee in {department.Department} is: {department.AverageSalery} SEK/month\n");
        }
        Console.Write("Press any key when ready: ");
        Console.ReadKey();
    }
    public static void AddEmployee() //Metoden för att lägga till data i Employee tabellen
    {
        AppDbContext dbContext = new AppDbContext();
        
            Console.WriteLine("Please fill out these details to add a new employee to the database");
            Console.Write("First name: ");
            string empFirstName = Console.ReadLine();

            Console.Write("Last name: ");
            string empLastName = Console.ReadLine();

            Console.Write("Date of birth (YYYYMMDD)");
            long DoB = GetUserInput();

            //Då min HiredDate är av datatypen DateTime så måste jag se till att vi får in användarens input till just DateTime
            //och väljer då att köra en while loop för att detta måste ske innan vi går vidare
            DateTime result;
            while (true)
            {
                Console.Write("Hired date (YYYYMMDD): "); //Ber användaren fylla i år, månad och dag i denna ordning
                string userInputDate = Console.ReadLine();
                if (DateTime.TryParseExact(userInputDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out result))
                { //Kör här en IF sats som endast byts om detta lyckas. Kör med TryParseExact metoden för att "rätta" userInputDate och den går ut till datatypen DateTime result sen.
                    break;
                }
                else
                {
                    Console.WriteLine("You made a invalid input");
                }
            }
            int profID = 0;
            int profIDUserInput;
            do //Då jag också har FK_ProfessionID i min Employee tabell som styr vilket yrke som den anställda har så hårdkodar jag en lista över dem yrkena
            { //som användaren får välja  vilken anställning den nya employee ska ha
                Console.WriteLine("Please chose which profession this employee are hired as: ");
                Console.WriteLine($"(0) - Principal");
                Console.WriteLine($"(1) - Vice Principal");
                Console.WriteLine($"(2) - IT Responsible");
                Console.WriteLine($"(3) - Financial Manager");
                Console.WriteLine($"(4) - Teacher");
                Console.WriteLine($"(5) - Substitute Teacher");
                profIDUserInput = GetUserInput();
                if (profIDUserInput < 6) //För att säkerställa att det inte blir en siffa som ej existerar så har jag gjort en IF sats att userInput måste vara mindre än 6 för att den ska
                {                       //bryta loopen. Mina FkProfessionId går dock från 1-6 så jag lägger till +1 på vad än användaren skriver in.
                    profID = profIDUserInput + 1;
                    break;
                }
                else
                {
                    Console.WriteLine("You have made a invalid input");
                }
            } while (true);

            string role = "";
            Console.WriteLine("New employee added to database:");
            switch (profID)
            {
                case 1:
                    role = "Principal";
                    break;
                case 2:
                    role = "Vice Principal";
                    break;
                case 3:
                    role = "IT Responsible";
                    break;
                case 4:
                    role = "Financial Manager";
                    break;
                case 5:
                    role = "Teacher";
                    break;
                case 6:
                    role = "Substitute Teacher";
                    break;
                default:
                    break;
            }   //Då jag vill ge en utskrift av all data som kommer spara in i databasen så tar jag profID genom en switch sats med samma hårkodade yrkes roll namn
                //för att kunna få med det i utskriften nedan
            Console.WriteLine($"Name: {empLastName}, {empFirstName}\nDate of birth: {DoB}\n" +
                $"Hired as: {role}\nFirst day of work: {result}");

            var emp = new Employee //Här skapar jag då ett nytt Employee objekt av all data jag samlta in
            {
                EmpFirstName = empFirstName,
                EmpLastName = empLastName,
                EmpDoB = DoB,
                HiredDate = result,
                FkProfessionId = profID
            };
            dbContext.Employees.Add(emp); //Slutligen så lägger jag till det nya Employee objektet till databasen
            //dbContext.SaveChanges();     //och spara ändringarna
            Console.WriteLine("Press any key when done: ");
            Console.ReadKey();
    }
    public static void AddStudent()
    {
        AppDbContext dbContext = new AppDbContext();
        //Skapar en student

        Console.Clear();
        Console.WriteLine("Please fill out these details to add a new student to the database");
        Console.Write("First name: ");
        string stuFirstName = Console.ReadLine();
        Console.Write("Last name: ");
        string stuLastName = Console.ReadLine();
        Console.Write("Date of birth (YYYYMMDD)");
        long stuDoB = GetUserInput();
        var stu = new Student
        {
            StuFirstName = stuFirstName,
            StuLastName = stuLastName,
            StuDoB = stuDoB
        };


        int saveChoice;
        do
        {
            Console.WriteLine("---------------");
            Console.WriteLine($"\nName: {stuLastName}, {stuFirstName} with date of birth: {stuDoB}\n");
            Console.WriteLine("---------------");

            Console.WriteLine("(1) - Save");
            Console.WriteLine("(0) - Exit without saving");
            Console.Write("Please confirm all the details above before you save this to the database: ");
            saveChoice = GetUserInput();

            switch (saveChoice)
            {
                case 1:
                    dbContext.Students.Add(stu);
                    dbContext.SaveChanges();
                    Console.WriteLine("Database updated!");
                    break;
                case 0:
                    Console.WriteLine("No changes added!");
                    break;
                default:
                    Console.WriteLine("You need to enter eihter 1 or 0 please.");
                    break;
            }
        } while (saveChoice >= 2);
        Console.WriteLine("Press any key when done: ");
        Console.ReadKey();
        Console.Clear();

        //Låter användaren lägga in studenten i en klass:
        if (saveChoice == 1)
        {
            do
            {
                Console.WriteLine("Please select which class the stundent will belong to.");
                Console.WriteLine("Would you like to take a look at the current students in each class first");
                Console.WriteLine("(1) - Yes");
                Console.WriteLine("(0) - No");
                Console.Write("Please enter choice: ");
                int viewClass = GetUserInput();
                switch (viewClass)
                {
                    case 1:
                        Console.Clear();
                        PrintStudentsInClasses();
                        break;
                    case 0:
                        Console.Clear();
                        Console.Write($"Please select which class you would like to add the new student {stuFirstName} {stuLastName} to\n");
                        break;
                    default:
                        Console.WriteLine("You need to enter 1 or 0 please");
                        break;
                }

                var classList = dbContext.Classes.ToList();
                int i = 1;
                foreach (var c in classList) //Visar upp alla klasser som användare kan tilldela eleven
                {
                    Console.WriteLine($"({i++}) - {c.ClassName}");
                }
                var selectedClass = GetUserInput() - 1; //Plockar ut den som användaren väljer
                Console.Clear();

                //Enrollar in denna student i kurser
                var activeCourses = dbContext.Courses.Where(s => s.CourseStatus == "A").ToList(); //Sparar ner alla kurser som har status A (aktiva) för att visa upp för användaren
                int y = 1;
                foreach (var course in activeCourses) //Visar upp alla kurser och lägger till en synlig siffra
                {
                    Console.WriteLine($"({y++}) - {course.SubjectName}");
                }
                Console.Write($"Please select which course you want to enroll student {stuFirstName} {stuLastName} in: ");
                var selectedCourse = GetUserInput() - 1; //Tar ut den kurs som användare har valt
                Console.Clear();

                Console.WriteLine("---------------");
                Console.WriteLine($"Student: {stuFirstName} {stuLastName} - DoB: {stuDoB}\n" +
                    $"Class: {classList[selectedClass].ClassName}\nCourse: {activeCourses[selectedCourse].SubjectName}");
                Console.WriteLine("---------------");

                Console.WriteLine("(1) - Save");
                Console.WriteLine("(2) - Restart enrollment");
                Console.WriteLine("(0) - Exit without saving");
                Console.Write("Please confirm all the details above before you save this to the database: ");
                int courseConf = GetUserInput();
                if (courseConf == 1)
                {
                    var enrollStu = new EnrollmentList //Om man väljer Save så sparar jag ner studentID från den student man skapat
                    {                                 //och tilldelar FK nycklarna till det som vi tagit ut ovan   
                        FkStudentId = stu.StudentId,
                        FkClassId = classList[selectedClass].ClassId,
                        FkCourseId = activeCourses[selectedCourse].CourseId,
                    };
                    dbContext.EnrollmentLists.Add(enrollStu);
                    dbContext.SaveChanges();
                    break; //Sparar allt och bryter loopen.
                }
                else if (courseConf == 2)
                {
                    Console.Clear();
                }
                else if (courseConf == 0)
                {
                    Console.Clear();
                    Console.WriteLine("You can always enroll students in courses and classes in the main menu instead,");
                    Console.Write("press any key to exit to main menu: ");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.Write("Invalid input, press any key to go back: ");
                    Console.ReadKey();
                }
            } while (true);
        }
        else
        {
            Console.Write("Press any key to exit to main menu: ");
            Console.ReadKey();
        }
    } //Metod för att lägga till ny Student
    public static void PrintAllStudents()
    {
        AppDbContext dbContext = new AppDbContext();
        var allStudents = from student in dbContext.Students
                         orderby student.StuLastName
                         select new
                         {
                             sDoB = student.StuDoB,
                             sFirstName = student.StuFirstName,
                             sLastName = student.StuLastName,  
                         };
        int i = 1;
        Console.Clear();
        foreach (var p in allStudents)
        {
            Console.WriteLine($"-- {i++} ----------\n{p.sLastName}, {p.sFirstName}\nDate of Birth: {p.sDoB}");
            Console.WriteLine("---------------");
        }
        Console.Write("Press any key when ready: ");
        Console.ReadKey();

    }
    public static void PrintStudentsInClasses()
    {
        Console.Clear();
        AppDbContext dbContext = new AppDbContext();

        var studentsInClass = from sClass in dbContext.Classes
                              join enroll in dbContext.EnrollmentLists on sClass.ClassId equals enroll.FkClassId
                              join student in dbContext.Students on enroll.FkStudentId equals student.StudentId
                              orderby student.StuLastName
                              select new
                              {
                                  cName = sClass.ClassName,
                                  sFirstName = student.StuFirstName,
                                  sLastName = student.StuLastName
                              };

        int i = 1;
        Console.WriteLine("--- BinaryBuddies ---\n");
        foreach (var p in studentsInClass.Where(c => c.cName == "BinaryBuddies"))
        {
            Console.WriteLine($"{i++}. {p.sLastName}, {p.sFirstName}");
        }
        int y = 1;
        Console.WriteLine("\n--- GitRDone ---\n");
        foreach (var p in studentsInClass.Where(c => c.cName == "GitRDone"))
        {
            Console.WriteLine($"{y++}. {p.sLastName}, {p.sFirstName}");
        }
        int j = 1;
        Console.WriteLine("\n--- CodeSlingers ---\n");
        foreach (var p in studentsInClass.Where(c => c.cName == "CodeSlingers"))
        {
            Console.WriteLine($"{j++}.  {p.sLastName}, {p.sFirstName}");
        }
        Console.WriteLine("\n--- TheC#s ---\n");
        int k = 1;
        foreach (var p in studentsInClass.Where(c => c.cName == "TheC#s"))
        {
            Console.WriteLine($"{k++}.  {p.sLastName}, {p.sFirstName}");
        }
        Console.WriteLine("Press any key when done: ");
        Console.ReadKey();
    }
    public static void PrintACourses()
    {
        AppDbContext dbContext = new AppDbContext();
        var activeCourses = from courses in dbContext.Courses
                            where courses.CourseStatus == "A"
                            orderby courses.SubjectName
                            select new
                            {
                                subject = courses.SubjectName
                            };
        int i = 1;
        foreach (var course in activeCourses)
        {
            Console.WriteLine($"{i++}. {course.subject}");
        }
        Console.Write("Press any key when ready: ");
        Console.ReadKey();
    }
    public static void PrintNaCourses()
    {
        AppDbContext dbContext = new AppDbContext();
        var activeCourses = from courses in dbContext.Courses
                            where courses.CourseStatus == "NA"
                            orderby courses.SubjectName
                            select new
                            {
                                subject = courses.SubjectName
                            };
        int i = 1;
        foreach (var course in activeCourses)
        {
            Console.WriteLine($"{i++}. {course.subject}");
        }
        Console.Write("Press any key when ready: ");
        Console.ReadKey();
    }
    public static void SetGrade()
    {
        AppDbContext dbContext = new AppDbContext();
        var allEnrollments = (from enrollments in dbContext.EnrollmentLists
                              join students in dbContext.Students on enrollments.FkStudentId equals students.StudentId
                              join employee in dbContext.Employees on enrollments.FkEmployeeId equals employee.EmployeeId
                              join courses in dbContext.Courses on enrollments.FkCourseId equals courses.CourseId
                              where enrollments.Grade == null
                              select new
                              {
                                  sFirstName = students.StuFirstName,
                                  sLastName = students.StuLastName,
                                  enrollID = enrollments.EnrollmentId,
                                  efkClassID = enrollments.FkClassId,
                                  efkEmployeeID = enrollments.FkEmployeeId,
                                  efkStudentID = enrollments.FkStudentId,
                                  efkCourseID = enrollments.FkCourseId,
                                  sGrade = enrollments.Grade,
                                  gradeDate = enrollments.GradeDate,
                                  eFirstName = employee.EmpFirstName,
                                  eLastName = employee.EmpLastName,
                                  subject = courses.SubjectName
                              }).ToList();
        try
        {
            using (var transaction = dbContext.Database.BeginTransaction()) //Jag använder mig av en transaction här enligt önskemål.
            {
                try
                {
                    while (true)
                    {
                        int i = 1;
                        int addedGrade;
                        foreach (var s in allEnrollments)
                        {
                            Console.WriteLine($"({i++}) - {s.sLastName}, {s.sFirstName} - {s.subject}");
                        }
                        Console.Write("Please select which enrollment/student you would like to set grade for: ");
                        var studentToGrade = GetUserInput() - 1;

                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine($"You have selected: {allEnrollments[studentToGrade].sLastName}, {allEnrollments[studentToGrade].sFirstName}" +
                            $" in course: {allEnrollments[studentToGrade].subject}");
                            Console.Write("Please select a grade from 1-5, where 5 is the highest grade there is: ");
                            addedGrade = GetUserInput();
                            if (addedGrade <= 5)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You need to enter 1-5 please.");
                                Console.Write("Press any key when ready: ");
                                Console.ReadKey();
                            }
                        }
                        Console.Clear();

                        Console.WriteLine($"You will add grade {addedGrade} to course {allEnrollments[studentToGrade].subject} for" +
                            $" student: {allEnrollments[studentToGrade].sLastName}, {allEnrollments[studentToGrade].sFirstName}\n");
                        Console.WriteLine($"The grade will be set by employee: {allEnrollments[studentToGrade].eLastName}, {allEnrollments[studentToGrade].eFirstName}" +
                            $" since this person is listed as the teacher for this subject." +
                            $" Grade date will be set as todays date: {DateTime.Now} This can't be changed.");
                        Console.WriteLine("If you would like to change this please contact system administrator for update of the course plan\n");
                        Console.WriteLine("(1) - Save");
                        Console.WriteLine("(0) - Exit without saving");
                        Console.Write("Please confirm all the details above before you save this to the database: ");
                        int userConf = GetUserInput();
                        if (userConf == 1)
                        {
                            DateTime date = DateTime.Now;
                            var updatedEnroll = new EnrollmentList
                            {
                                EnrollmentId = allEnrollments[studentToGrade].enrollID,
                                FkStudentId = allEnrollments[studentToGrade].efkStudentID,
                                FkEmployeeId = allEnrollments[studentToGrade].efkEmployeeID,
                                FkClassId = allEnrollments[studentToGrade].efkClassID,
                                FkCourseId = allEnrollments[studentToGrade].efkCourseID,
                                Grade = addedGrade,
                                GradeDate = date,
                            };
                            dbContext.Update(updatedEnroll); 
                            dbContext.SaveChanges();
                            transaction.Commit();

                            Console.WriteLine("Database updated!");
                            Console.Write("Press any key when ready: ");
                            Console.ReadKey();
                            break;
                        }
                        else if (userConf == 0)
                        {
                            Console.WriteLine("No changes added!");
                            Console.Write("Press any key when ready: ");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input, menu reboot.");
                            Console.Write("Press any key when ready: ");
                            Console.ReadKey();
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback(); //Om det inte skulle lyckas så kommer den köra en Rollback här!
                    throw e;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Transaction not completed, see {e.Message}");
            throw;
        }
    }
    public static void ChangeGrade()
    {
        AppDbContext dbContext = new AppDbContext();
        var allEnrollments = (from enrollments in dbContext.EnrollmentLists
                              join students in dbContext.Students on enrollments.FkStudentId equals students.StudentId
                              join employee in dbContext.Employees on enrollments.FkEmployeeId equals employee.EmployeeId
                              join courses in dbContext.Courses on enrollments.FkCourseId equals courses.CourseId
                              where enrollments.Grade <= 5 
                              select new 
                              {
                                  sFirstName = students.StuFirstName,
                                  sLastName = students.StuLastName,
                                  enrollID = enrollments.EnrollmentId,
                                  efkClassID = enrollments.FkClassId,
                                  efkEmployeeID = enrollments.FkEmployeeId,
                                  efkStudentID = enrollments.FkStudentId,
                                  efkCourseID = enrollments.FkCourseId,
                                  sGrade = enrollments.Grade,
                                  gradeDate = enrollments.GradeDate,
                                  eFirstName = employee.EmpFirstName,
                                  eLastName = employee.EmpLastName,
                                  subject = courses.SubjectName
                              }).ToList();
        while (true)
        {
            int i = 1;
            foreach (var s in allEnrollments)
            {
                Console.WriteLine($"-- ({i++}) -----------\n{s.sLastName}, {s.sFirstName} - {s.subject}\n" +
                    $"Current grade: {s.sGrade}\nGrade was given: {s.gradeDate}\n");
            }
            Console.Write("Please select which enrollment/student you would like change grade for: ");
            var studentToGrade = GetUserInput() - 1;
            int changedGrade;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"You have selected: {allEnrollments[studentToGrade].sLastName}, {allEnrollments[studentToGrade].sFirstName}" +
                $" in course: {allEnrollments[studentToGrade].subject} with current grade: {allEnrollments[studentToGrade].sGrade}");
                Console.Write("Please select a grade from 1-5, where 5 is the highest grade there is: ");
                changedGrade = GetUserInput();
                if (changedGrade <= 5)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You need to enter 1-5 please.");
                    Console.Write("Press any key when ready: ");
                    Console.ReadKey();
                }
            }
            Console.Clear();
            Console.WriteLine($"You will change grade from {allEnrollments[studentToGrade].sGrade} to {changedGrade} " +
                $"in course {allEnrollments[studentToGrade].subject} for" +
                $" student: {allEnrollments[studentToGrade].sLastName}, {allEnrollments[studentToGrade].sFirstName}\n");
            Console.WriteLine($"The grade will be set by employee: {allEnrollments[studentToGrade].eLastName}, {allEnrollments[studentToGrade].eFirstName}" +
                $" since this person is listed as the teacher for this subject." +
                $" Grade date will be set as todays date: {DateTime.Now} This can't be changed.");
            Console.WriteLine("If you would like to change this please contact system administrator for update of the course plan\n");
            Console.WriteLine("(1) - Save");
            Console.WriteLine("(0) - Exit without saving");
            Console.Write("Please confirm all the details above before you save this to the database: ");
            int userConf = GetUserInput();
            if (userConf == 1)
            {
                DateTime date = DateTime.Now;
                var updatedEnroll = new EnrollmentList
                {
                    EnrollmentId = allEnrollments[studentToGrade].enrollID,
                    FkStudentId = allEnrollments[studentToGrade].efkStudentID,
                    FkEmployeeId = allEnrollments[studentToGrade].efkEmployeeID,
                    FkClassId = allEnrollments[studentToGrade].efkClassID,
                    FkCourseId = allEnrollments[studentToGrade].efkCourseID,
                    Grade = changedGrade,
                    GradeDate = date,
                };
                dbContext.Update(updatedEnroll);
                dbContext.SaveChanges();
                Console.WriteLine("Database updated!");
                Console.Write("Press any key when ready: ");
                Console.ReadKey();
                break;
            }
            else if (userConf == 0)
            {
                Console.WriteLine("No changes added!");
                Console.Write("Press any key when ready: ");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("Invalid input, menu reboot.");
                Console.Write("Press any key when ready: ");
                Console.ReadKey();
            }
        }
    }
    public static void EnrollStudent()
    {
        AppDbContext dbContext = new AppDbContext();
        var availableClasses = (from sClass in dbContext.Classes  //Jag väljer att skapa lite olika listor för jag vill inte få dubbla utskrifter när man ska 
                               select new                         //få välja ut vilken klass eller vilken student man vill hantera
                               {
                                   className = sClass.ClassName,
                                   classID = sClass.ClassId
                               }).ToList();

        var availableStudents = (from student in dbContext.Students
                                select new
                                {
                                    sFirstName = student.StuFirstName,
                                    sLastName = student.StuLastName,
                                    sDoB = student.StuDoB,
                                    sID = student.StudentId
                                }).ToList();
        var availableCourses = (from course in dbContext.Courses
                               where course.CourseStatus == "A"
                               select new
                               {
                                   courseName = course.SubjectName,
                                   courseID = course.CourseId
                               }).ToList();

        var enrolledStudents = (from student in dbContext.Students
                               join enrollment in dbContext.EnrollmentLists on student.StudentId equals enrollment.FkStudentId
                               join employee in dbContext.Employees on enrollment.FkEmployeeId equals employee.EmployeeId
                               join courses in dbContext.Courses on enrollment.FkCourseId equals courses.CourseId
                               join sClass in dbContext.Classes on enrollment.FkClassId equals sClass.ClassId
                               select new
                               {
                                   sFirstName = student.StuFirstName,
                                   sLastName = student.StuLastName,
                                   className = sClass.ClassName,
                                   studentID = student.StudentId,
                                   enrollID = enrollment.EnrollmentId,
                                   efkClassID = enrollment.FkClassId,
                                   efkEmployeeID = enrollment.FkEmployeeId,
                                   efkStudentID = enrollment.FkStudentId,
                                   efkCourseID = enrollment.FkCourseId,
                                   sGrade = enrollment.Grade,
                                   gradeDate = enrollment.GradeDate,
                                   eFirstName = employee.EmpFirstName,
                                   eLastName = employee.EmpLastName,
                                   subject = courses.SubjectName
                               }).ToList();




        while (true)
        {
            int j = 1;
            foreach (var s in availableStudents)
            {  
                Console.WriteLine($"-- ({j++}) ----------\n{s.sLastName}, {s.sFirstName}\nDate of birth: {s.sDoB}");
            }
            Console.Write("Please select which student you would like to enroll: ");
            var selectedStudent = GetUserInput() - 1; //Tar ut vilken student som man vill enrolla.

            //Här kollar jag om den elev som valts ut redan finns med i min EnrollmentList och beroende på det så får man lägga till en klass på eleven
            //samt att jag vill visa upp vilka kurser den redan går om den nu redan finns med i den tabellen
            var result = enrolledStudents.Find(x => x.efkStudentID == availableStudents[selectedStudent].sID);

            if (result == null)
            {
                Console.WriteLine($"Student {availableStudents[selectedStudent].sLastName}, {availableStudents[selectedStudent].sFirstName} is not enrolled in any courses today and is not" +
                $" added to a class either!\n");
                Console.WriteLine("Press any key to continue: ");
                Console.ReadKey();

                int m = 1;
                foreach (var c in availableClasses)
                {
                    Console.WriteLine($"({m++}). {c.className}");
                }
                Console.WriteLine($"Please select which class to enroll {availableStudents[selectedStudent].sLastName}, {availableStudents[selectedStudent].sFirstName} in: ");
                var selectedClass = GetUserInput() - 1;

                int k = 1;
                foreach (var c in availableCourses)
                {
                    Console.WriteLine($"({k++}). {c.courseName}");
                }
                Console.WriteLine("This is all the available courses, please select which course you would like to enroll student in: ");
                var selectedCourse = GetUserInput() - 1; //Tar ut vilken kurs man vill enrolla till.

                Console.WriteLine($"You have selected to enroll student: {availableStudents[selectedStudent].sLastName}, {availableStudents[selectedStudent].sFirstName}\n" +
                    $"in class: {availableClasses[selectedClass].className}\nto course: {availableCourses[selectedCourse].courseName}");
                Console.WriteLine("(1). Save changes");
                Console.WriteLine("(0). Exit without saving");
                Console.WriteLine("Please confirm to save changes to database: ");
                int userConf = GetUserInput();
                if (userConf == 1)
                {
                    var newEnrollment2 = new EnrollmentList
                    {
                        FkStudentId = availableStudents[selectedStudent].sID,
                        FkClassId = availableClasses[selectedClass].classID,
                        FkCourseId = availableCourses[selectedCourse].courseID
                    };
                    dbContext.Add(newEnrollment2);
                    dbContext.SaveChanges();
                    Console.WriteLine("Database updated!");
                    Console.Write("Press any key to exit to main menu: ");
                    Console.ReadKey();
                    break;
                }
                else if (userConf == 0)
                {
                    Console.WriteLine("No changes will be made to database!");
                    Console.Write("Press any key to exit to main menu: ");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("You need to enter either 1 or 0");
                    Console.Write("Press any key to restart enrollment process: ");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Clear();

                Console.WriteLine($"This is all the courses student {availableStudents[selectedStudent].sLastName}, {availableStudents[selectedStudent].sFirstName} is enrolled in today:\n");
                foreach (var enroll in enrolledStudents.Where(x => x.efkStudentID == availableStudents[selectedStudent].sID))
                {
                    Console.WriteLine($"{enroll.subject}");
                }
                Console.WriteLine("\nThis is all the available courses: ");
                int l = 1;
                foreach (var c in availableCourses)
                {
                    Console.WriteLine($"({l++}). {c.courseName}");
                }
                Console.Write("Please select which course you would like to enroll student in: ");
                var selectedCourse = GetUserInput() - 1; //Tar ut vilken kurs man vill enrolla till.

                Console.WriteLine($"You have selected to enroll student: {availableStudents[selectedStudent].sLastName}, {availableStudents[selectedStudent].sFirstName}\n" +
                $"to course: {availableCourses[selectedCourse].courseName}");

                Console.WriteLine("(1). Save changes");
                Console.WriteLine("(0). Exit without saving");
                Console.WriteLine("Please confirm to save changes to database: ");
                int userConf = GetUserInput();
                if (userConf == 1)
                {
                    var newEnrollment = new EnrollmentList
                    {
                        FkStudentId = availableStudents[selectedStudent].sID,
                        FkCourseId = availableCourses[selectedCourse].courseID,
                        FkClassId = enrolledStudents[selectedStudent].efkClassID
                    };
                    dbContext.Add(newEnrollment);
                    dbContext.SaveChanges();
                    Console.WriteLine("Database updated!");
                    Console.Write("Press any key to exit to main menu: ");
                    Console.ReadKey();
                    break;
                }
                else if (userConf == 0)
                {
                    Console.WriteLine("No changes will be made to database!");
                    Console.Write("Press any key to exit to main menu: ");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("You need to enter either 1 or 0");
                    Console.Write("Press any key to restart enrollment process: ");
                    Console.ReadKey();
                }
            }
        }
    } //Metod för att enrolla en student i nya kurser samt om man ej satt en klass på en elev får man enrolla den med.
    public static int GetUserInput() //Då jag ska ta in mycket userInput's i heltal så gjorde jag tidigt en metod för just detta
    {
        int userChoice;
        while (true)
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out userChoice))
            {
                break;
            }
            else
            {
                Console.WriteLine("You need to print a number input");
            }
        }
        return userChoice;
    }
    public static void Logo() //Tyckte det skulle vara lite roligt med en logo så lekte lite med det också.
    {
        string logo = @"         
         _______
        |.-----.|  The
        ||     ||  School
        ||_____||  Of
        `--)-(--`  Programming
       __[=== o]___ -> T.S.O.P
      |:::::::::::|\ 
      `-=========-`()";
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(logo);
        Console.ResetColor();
    }
}



