using System;
using NUnit.Framework;

namespace Sharp.Domain.Trading;

[TestFixture]
public class WalletTest
{
    private Wallet _wallet;

    [Test]
    public void ShouldThrowOnNegativeDeposit()
    {
        _wallet = new Wallet(50);
        Assert.Throws<ArgumentException>(() => _wallet.Deposit(-23));
    }
    
    [Test]
    public void ShouldThrowOnNegativeCharge()
    {
        _wallet = new Wallet(60);
        Assert.Throws<ArgumentException>(() => _wallet.Charge(-100));
    }
    
    [Test]
    public void ShouldDepositAmount()
    {
        _wallet = new Wallet(80);
        _wallet.Deposit(12);
        Assert.AreEqual(92, _wallet.Balance);
    }
    
    [Test]
    public void ShouldChargeAmount()
    {
        _wallet = new Wallet(100);
        _wallet.Charge(1);
        Assert.AreEqual(99, _wallet.Balance);
    }
}