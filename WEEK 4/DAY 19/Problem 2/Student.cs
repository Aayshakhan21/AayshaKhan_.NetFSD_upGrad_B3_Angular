using System;

class Student
{
    public double CalculateAverage(int m1, int m2, int m3)
    {
        return (m1 + m2 + m3) / 3.0;
    }

    public char GetGrade(double avg)
    {
        if (avg >= 80)
            return 'A';
        else if (avg >= 60)
            return 'B';
        else if (avg >= 40)
            return 'C';
        else
            return 'F';
    }

    static void Main()
    {
        Student s = new Student();

        Console.WriteLine("Enter Marks:");

        string input = Console.ReadLine();
        string[] marks = input.Split(' ');

        if (marks.Length != 3)
            throw new Exception("Please enter exactly 3 marks.");

        int m1, m2, m3;

        if (!int.TryParse(marks[0], out m1) ||
            !int.TryParse(marks[1], out m2) ||
            !int.TryParse(marks[2], out m3))
        {
            throw new Exception("Invalid input. Marks must be numbers.");
        }

        if (m1 < 0 || m2 < 0 || m3 < 0) {
            throw new Exception("Marks cannot be negative.");
        }

        double avg = s.CalculateAverage(m1, m2, m3);
        char grade = s.GetGrade(avg);

        Console.WriteLine("Average = " + avg + ", Grade = " + grade);
    }
}