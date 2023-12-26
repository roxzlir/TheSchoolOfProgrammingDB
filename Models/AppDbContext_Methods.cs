﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSchoolOfProgrammingDB.Models;

public partial class AppDbContext_Methods : DbContext
{
    public static void PrintEmployees()
    {
        AppDbContext dbContext = new AppDbContext();
        var empInfo = from employee in dbContext.Employees
                      join profession in dbContext.Professions on employee.FkProfessionId equals profession.ProfessionId
                      orderby profession.ProfessionId
                      select new
                      {
                          eFirstName = employee.EmpFirstName,
                          eLastName = employee.EmpLastName,
                          proName = profession.ProTitle,
                          yearsHired = EF.Functions.DateDiffYear(employee.HiredDate, DateTime.Now),
                          dateHired = employee.HiredDate
                      };



        foreach (var employee in empInfo)
        {
            DateTime date = Convert.ToDateTime(employee.dateHired); //Ville få bort dem tomma klockslagen från utskriften så sparar ner alla datum från employee.dateHired till
            string dateOnly = date.ToString("yyyy-MM-dd");  //date och gör sedan om dem till en string med formatet jag önskar.
            Console.WriteLine($"Employee: {employee.eFirstName} {employee.eLastName}\nWork title: {employee.proName}\nYears hired: {employee.yearsHired}\n" +
                $"Employed since: {dateOnly})\n-----------------------");
        }
        Console.WriteLine("Press any key when done: ");
        Console.ReadKey();
    }
    public static void PrintEmpDepartments()
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
    }
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
            Console.WriteLine($"{i++}. {p.sLastName}, {p.sFirstName}\nDate of Birth: {p.sDoB}");
            Console.WriteLine("---------------");
        }
        Console.Write("Press any key when ready: ");
        Console.ReadKey();

    }
    public static void PrintStudentsInClasses()
    {
        AppDbContext dbContext = new AppDbContext();

        var stuInClass = from enrollment in dbContext.EnrollmentLists
                           join student in dbContext.Students on enrollment.FkStudentId equals student.StudentId
                           join classes in dbContext.Classes on enrollment.FkClassId equals classes.ClassId
                           orderby student.StuLastName
                           select new
                           {
                               cName = classes.ClassName,
                               sFirstName = student.StuFirstName,
                               sLastName = student.StuLastName
                           };

        int i = 1;
        Console.WriteLine("--- BinaryBuddies ---\n");
        foreach (var p in stuInClass.Where(c => c.cName == "BinaryBuddies"))
        {
            Console.WriteLine($"{i++} {p.sLastName}, {p.sFirstName}");
        }
        int y = 1;
        Console.WriteLine("\n--- GitRDone ---\n");
        foreach (var p in stuInClass.Where(c => c.cName == "GitRDone"))
        {
            Console.WriteLine($"{y++} {p.sLastName}, {p.sFirstName}");
        }
        int j = 1;
        Console.WriteLine("\n--- CodeSlingers ---\n");
        foreach (var p in stuInClass.Where(c => c.cName == "CodeSlingers"))
        {
            Console.WriteLine($"{j++} {p.sLastName}, {p.sFirstName}");
        }
        Console.WriteLine("\n--- TheC#s ---\n");
        int k = 1;
        foreach (var p in stuInClass.Where(c => c.cName == "TheC#s"))
        {
            Console.WriteLine($"{k++} {p.sLastName}, {p.sFirstName}");
        }
        Console.WriteLine("Press any key when done: ");
        Console.ReadKey();
    }

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

}


