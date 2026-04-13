using System;

// Record 
record Student(int RollNo, string Name, string Course, int Marks);

class Program
{
    static void Main()
    {
        Console.Write("Enter number of students: ");
        int n = int.Parse(Console.ReadLine());

        // array to store value
        Student[] students = new Student[n];

        // 
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("\nEnter details:");

            int roll;
            while (true)
            {
                Console.Write("Roll Number: ");
                if (int.TryParse(Console.ReadLine(), out roll))
                    break;
                else
                    Console.WriteLine("Invalid Roll Number!");
            }

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Course: ");
            string course = Console.ReadLine();

            int marks;
            while (true)
            {
                Console.Write("Marks: ");
                if (int.TryParse(Console.ReadLine(), out marks))
                    break;
                else
                    Console.WriteLine("Invalid Marks!");
            }

            // store data
            students[i] = new Student(roll, name, course, marks);
        }

        Console.WriteLine("\nStudent Records:");
        foreach (var s in students)
        {
            Console.WriteLine($"Roll No: {s.RollNo} | Name: {s.Name} | Course: {s.Course} | Marks: {s.Marks}");
        }
        //search by roll no.

        Console.Write("\nEnter Roll Number to search: ");
        int searchRoll = int.Parse(Console.ReadLine());

        Student found = null;

        foreach (var s in students)
        {
            if (s.RollNo == searchRoll)
            {
                found = s;
                break;
            }
        }

        Console.WriteLine("\nSearch Result:");
        if (found != null)
        {
            Console.WriteLine("Student Found:");
            Console.WriteLine($"Roll No: {found.RollNo} | Name: {found.Name} | Course: {found.Course} | Marks: {found.Marks}");
        }
        else
        {
            Console.WriteLine("Student not found!");
        }
    }
}