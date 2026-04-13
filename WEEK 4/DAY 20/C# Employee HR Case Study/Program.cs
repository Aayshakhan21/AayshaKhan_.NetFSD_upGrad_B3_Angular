using ConsoleApp1;
using System;

namespace ConsoleApp35
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Employee emp = new Employee("John Elia", 4500m, 35, "E2323");
            emp.PrintDetails();
            //Console.WriteLine(emp.Salary);

        }
    }
}