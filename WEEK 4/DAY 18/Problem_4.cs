using System;
namespace ConsoleApp2
{
    internal class Problem_4
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Number: ");
            int n = int.Parse(Console.ReadLine());

            int evenCount = 0;
            int oddCount = 0;
            int sum = 0;

            for (int i = 1; i <= n; i++)
            {
                sum += i;

                if (i % 2 == 0)
                    evenCount++;
                else
                    oddCount++;
            }

            Console.WriteLine("Even Count: " + evenCount);
            Console.WriteLine("Odd Count: " + oddCount);
            Console.WriteLine("Sum: " + sum);
        }
    }
}
