using System;

namespace ConsoleApp2
{
    internal class Problem_2
    {
        static void Main(string[] args)
        {
            Console.Write("Enter First Number: ");
            string input1 = Console.ReadLine();

            double num1;

            if (!double.TryParse(input1, out num1))
            {
                Console.WriteLine("Please enter a valid number.");
                return;
            }

            Console.Write("Enter Second Number: ");
            string input2 = Console.ReadLine();

            double num2;

            if (!double.TryParse(input2, out num2))
            {
                Console.WriteLine("Please enter a valid number.");
                return;
            }

            Console.Write("Enter Operator (+, -, *, /): ");
            char op = char.Parse(Console.ReadLine());

            double result = 0;

            switch (op)
            {
                case '+':
                    result = num1 + num2;
                    break;

                case '-':
                    result = num1 - num2;
                    break;

                case '*':
                    result = num1 * num2;
                    break;

                case '/':
                    if (num2 == 0)
                    {
                        Console.WriteLine("Division by zero not allowed");
                        return;
                    }
                    result = num1 / num2;
                    break;

                default:
                    Console.WriteLine("Invalid Operator");
                    return;
            }

            Console.WriteLine("Result: " + Math.Round(result, 2));
        }
    }
}

