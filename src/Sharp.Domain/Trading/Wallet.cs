namespace Sharp.Domain.Trading;

/// <summary>
/// Wallet that holds money of the player
/// </summary>
public class Wallet
{
    private int _balance;

    public Wallet(int balance)
    {
        _balance = balance;
    }

    /// <summary>
    /// Charge an amount of money
    /// </summary>
    /// <param name="amount">non-negative amount that should be charged on the wallet</param>
    /// <exception cref="ArgumentException">If amount is negative</exception>
    public void Charge(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Charge amount cannot be negative", nameof(amount));
        _balance -= amount;        
    }

    /// <summary>
    /// Deposit an amount of money
    /// </summary>
    /// <param name="amount">non-negative amount that should be deposited</param>
    /// <exception cref="ArgumentException">If amount is negative</exception>
    public void Deposit(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Deposit amount cannot be negative", nameof(amount));
        _balance += amount;
    }

    /// <summary>
    /// Balance
    /// </summary>
    public int Balance => _balance;
}