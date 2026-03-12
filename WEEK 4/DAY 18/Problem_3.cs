namespace ConsoleApp2
{
    internal class Problem_3
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Salary: ");
            double salary = double.Parse(Console.ReadLine());

            Console.Write("Enter Experience (in years): ");
            int exp = int.Parse(Console.ReadLine());

            double bonus;

            if (exp < 2)
                bonus = salary * 0.05;
            else if (exp >= 2 && exp <= 5)
                bonus = salary * 0.10;
            else
                bonus = salary * 0.15;

            double finalSalary = salary + bonus;

            Console.WriteLine("Employee: " + name);
            Console.WriteLine("Bonus: " + bonus);
            Console.WriteLine("Final Salary: " + finalSalary);
        }

    }
}