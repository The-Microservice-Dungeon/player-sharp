namespace Sharp.Gameplay.Trading;

public class Wallet
{
    private int _balance;

    public Wallet(int balance)
    {
        _balance = balance;
    }

    public void Charge(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Charge amount cannot be negative", nameof(amount));
        _balance -= amount;        
    }

    public void Deposit(int amount)
    {
        if (amount < 0)
            throw new ArgumentException("Deposit amount cannot be negative", nameof(amount));
        _balance -= amount;
    }

    public int Balance => _balance;
}