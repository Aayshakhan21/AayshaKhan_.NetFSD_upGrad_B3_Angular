using System;

class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }

    static void Main()
    {
        Calculator calc = new Calculator();

        Console.Write("Enter any Two Numbers : ");
        string input = Console.ReadLine();  
        string[] numbers = input.Split(' ');

        int a, b;

        if (!int.TryParse(numbers[0], out a) || !int.TryParse(numbers[1], out b))
        {
            Console.WriteLine("Invalid input");
            return;
        }

        int addResult = calc.Add(a, b);
        int subResult = calc.Subtract(a, b);

        Console.WriteLine($"Addition = {addResult}, Subtraction = {subResult}");
    }
}