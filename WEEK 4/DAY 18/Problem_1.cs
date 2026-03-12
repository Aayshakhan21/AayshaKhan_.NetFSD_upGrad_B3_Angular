using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    internal class Problem_1
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Student Marks: ");
            int marks = int.Parse(Console.ReadLine());

            if (marks < 0 || marks > 100)
            {
                Console.WriteLine("Invalid Marks");
            }
            else
            {
                string grade;

                if (marks >= 90)
                    grade = "A";
                else if (marks >= 75)
                    grade = "B";
                else if (marks >= 60)
                    grade = "C";
                else if (marks >= 40)
                    grade = "D";
                else
                    grade = "Fail";

                Console.WriteLine("Student: " + name);
                Console.WriteLine("Grade: " + grade);
            }
        }
    }
}
