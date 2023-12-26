using Microsoft.EntityFrameworkCore;
using TheSchoolOfProgrammingDB.Models;

namespace TheSchoolOfProgrammingDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDbContext_Methods.PrintAllStudents();
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
    }
