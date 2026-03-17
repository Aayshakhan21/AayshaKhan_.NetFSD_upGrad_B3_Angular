using System;

// custom exception
class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message) : base(message) { }
}

class BankAccount
{
    private double _balance;

    public BankAccount(double balance)
    {
        _balance = balance;
    }

    public void Withdraw(double amount)
    {
        if (amount > _balance)
        {
            throw new InsufficientBalanceException("Withdrawal amount exceeds available balance");
        }

        _balance -= amount;
        Console.WriteLine("Withdrawal successful! Remaining balance: " + _balance);
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter Balance: ");
        double bal = double.Parse(Console.ReadLine());

        Console.Write("Enter Withdraw Amount: ");
        double amt = double.Parse(Console.ReadLine());

        BankAccount account = new BankAccount(bal);

        try
        {
            account.Withdraw(amt);
        }
        catch (InsufficientBalanceException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Transaction completed.");
        }
    }
}