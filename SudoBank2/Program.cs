using System;
using System.Collections.Generic;  // ADDED for List support

// 1. Explain the line below.
public interface IRepository
{
    void Create(BankAccount account);
    BankAccount Read(string accountNumber);
    void Update(BankAccount account);
}

public abstract class AccountBase
{
    public string AccountNumber { get; set; }
    public string AccountHolder { get; set; }
    public double Balance { get; set; }

    // 2. CalculateInterest is abstract, what does that mean?
    public abstract double CalculateInterest();

    // 3. What does virtual mean in this context of GenerateStatement
    public virtual void GenerateStatement(DateTime startDate)
    {
        Console.WriteLine($"Generating statement starting from {startDate.ToShortDateString()}");
    }
}

// 4. Describe this class below. How do we call the method below.
public static class BankValidator
{
    public static void ValidateString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new FormatException();
        }
    }
}

// 5. Describe the inheritance mapping below and what it means.
// CONVERTED: Arrays → Lists
public partial class BankManager : IRepository
{
    // CHANGED: Array to List (no more size limit!)
    private List<BankAccount> _accounts = new List<BankAccount>();
    // REMOVED: private int _accountCount = 0; (List handles this automatically)

    public void Create(BankAccount account)
    {
        // 6. What does this line below do in this context.
        if (account == null)
        {
            throw new Exception("Account cannot be null");
        }
        // CHANGED: Using List.Add instead of array index
        _accounts.Add(account);
    }

    public BankAccount Read(string accountNumber)
    {
        // CHANGED: Using foreach instead of for loop with counter
        foreach (BankAccount account in _accounts)
        {
            if (account.AccountNumber == accountNumber)
            {
                return account;
            }
        }
        throw new Exception("Account not found");
    }

    public void Update(BankAccount account)
    {
        BankAccount existing = Read(account.AccountNumber);
        existing.AccountHolder = account.AccountHolder;
        existing.Balance = account.Balance;
    }

    // CHANGED: Delete method is now MUCH simpler with List
    public void Delete(string accountNumber)
    {
        // List.RemoveAll removes all matching accounts in one line!
        int removed = _accounts.RemoveAll(a => a.AccountNumber == accountNumber);
        if (removed == 0)
        {
            throw new Exception("Account not found");
        }
    }
}

// 7. Describe the inheritance below and the significance of AccountBase
public class BankAccount : AccountBase
{
    public double InterestRate { get; set; }

    public override double CalculateInterest()
    {
        return Balance * (InterestRate / 100);
    }

    public override void GenerateStatement(DateTime startDate)
    {
        Console.WriteLine($"Account: {AccountNumber} | Holder: {AccountHolder} | Balance: {Balance}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankManager manager = new BankManager();

        try
        {
            Console.WriteLine("--- Banking Management System ---");
            Console.Write("Enter Account Number: ");
            string accNum = Console.ReadLine();
            BankValidator.ValidateString(accNum);

            Console.Write("Enter Account Holder: ");
            string holder = Console.ReadLine();
            BankValidator.ValidateString(holder);

            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accNum,
                AccountHolder = holder,
                Balance = 7500.50,
                InterestRate = 5.25
            };

            manager.Create(newAccount);

            double interest = newAccount.CalculateInterest();
            Console.WriteLine($"\nRecord Stored. Projected Interest: R{interest}");

            newAccount.GenerateStatement(DateTime.Now);
            newAccount.GenerateStatement(DateTime.Now);

            Console.WriteLine("\nTesting Delete Operation...");
            manager.Delete(accNum);
            Console.WriteLine("Account removed successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Format not recognized.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}