using System;
using System.Xml.Linq;
namespace ConsoleApp1
{
    internal class Employee
    {
        //private variables

        private string _employeeId;
        private string _fullName;
        private int _age;
        private decimal _salary;

        //properties to set and get values
        public string EmployeeId
        {
            get {  return _employeeId ;}
        }
        public int Age
        {
            get { return _age; }
            set
            {
                if(value < 18 || value > 80)
                {
                    throw new ArgumentException("Age is invalid, must be between 18 and 80.");
                }
                _age = value;
            }
        }
        public decimal Salary
        {
            get { return _salary; }
            private set
            {
                if (value < 1000)
                {
                    throw new ArgumentException("Invalid salary, Minimum salary is INR.1000");
                }
                _salary = value;
            }
        }
        public string FullName{
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Employee Name cannot be empty");
                }
                _fullName = value;
            }
        }

        //constructor to initialize object
        public Employee(string fullName, decimal salary, int age, string employeeId = null)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                _employeeId = "E" + new Random().Next(1000, 9999);
            }
            else
            {
                _employeeId = employeeId;
            }
            FullName = fullName;
            Age = age;
            Salary = salary;
        }

        //give raise
        public void GiveRaise(decimal percentage)
        {
            if (percentage <= 0 || percentage > 30)
            {
                throw new ArgumentException("Raise percentage must be between 1 and 30.");
            }

            decimal increase = _salary * (percentage / 100);
            Salary = _salary + increase;

            Console.WriteLine($"Raise applied. New Salary: INR {Salary}");
        }

        //penalty amounts
        public bool DeductPenalty(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Penalty amount must be greater than zero.");
                return false;
            }

            if (_salary - amount < 1000)
            {
                Console.WriteLine("Penalty rejected. Salary cannot go below INR 1000.");
                return false;
            }

            Salary = _salary - amount;

            Console.WriteLine($"Penalty applied. Remaining Salary: INR {Salary}");
            return true;
        }

        // show employee details

        public void PrintDetails()
        {
            Console.WriteLine("Employee Details");
            Console.WriteLine();
            Console.WriteLine($"Employee ID : {EmployeeId}");
            Console.WriteLine($"Full Name   : {FullName}");
            Console.WriteLine($"Age         : {Age}");
            Console.WriteLine($"Salary      : INR {Salary}");
        }
    }
}
