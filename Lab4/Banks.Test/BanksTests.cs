using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Tools;
using NuGet.Frameworks;
using Xunit;
using Transaction = System.Transactions.Transaction;

namespace Banks.Test;

public class BanksTests
{
    private CentralBank CentralBank => CentralBank.GetInstance();

    [Fact]
    public void Transaction_TransferWorks()
    {
        var settings = new BankSettings(0.01M, 10000M, 0.01M, 0.02M, 0.03M, 0.01M, 5000M);
        var bank = CentralBank.CreateBank("SBER", settings);
        var client1 = new Client("Biba", "Boba", "pr. Nevsky", "1234567890");
        var client2 = new Client("Pipa", "Boba", "st. Dybenko ", "0987654321");
        bank.AddClient(client1);
        bank.AddClient(client2);

        var account1 = bank.CreateDebitAccount(client1);
        var account2 = bank.CreateDebitAccount(client2);
        account1.Deposit(200);
        account2.Deposit(100);
        var transaction = new TransferTransaction(account1, account2, 100, CentralBank.TimeManager.CentralBankTime);

        Assert.Equal(100, account1.Balance);
        Assert.Equal(200, account2.Balance);
    }

    [Fact]
    public void Transaction_WithdrawForSuspiciousClientWorks()
    {
        var settings = new BankSettings(0.01M, 10000M, 0.01M, 0.02M, 0.03M, 0.01M, 5000M);
        var bank = CentralBank.CreateBank("Tink", settings);
        var client1 = new Client("Thomas", "Shelby", " ", " ");
        bank.AddClient(client1);

        var account1 = bank.CreateDebitAccount(client1);
        account1.Deposit(10000);
        var exception = Record.Exception(() =>
            "Unverified client can't withdraw more than 5000");

        Assert.Throws<BanksException>(() =>
            new WithdrawTransaction(account1, 6000, CentralBank.TimeManager.CentralBankTime));
        Assert.Null(exception);
    }

    [Fact]
    public void ClientCanSubscribe_Notified()
    {
        var settings = new BankSettings(0.01M, 10000M, 0.01M, 0.02M, 0.03M, 0.01M, 5000M);
        var bank = CentralBank.CreateBank("Sigma Bank", settings);
        var client1 = new Client("Daniil", "Kurepin", "some address", "32189321");
        bank.AddClient(client1);

        var account1 = bank.CreateDebitAccount(client1);
        account1.Deposit(10000);
        bank.AddObserver(client1);
        bank.ChangeDebitInterest(0.02M);

        Assert.True(client1.Notifications.Count == 1);
        Assert.Equal("Bank Sigma Bank notifies you: Debit interest changed to 0.02", client1.Notifications[0]);
    }

    [Fact]
    public void DebitAccountPayouts_RewindTimeWorks()
    {
        var settings = new BankSettings(0.01M, 10000M, 0.01M, 0.02M, 0.03M, 0.01M, 5000M);
        var bank = CentralBank.CreateBank("Ural Bank", settings);
        var client1 = new Client("Top", "G", "Top G address", "123123123");
        bank.AddClient(client1);

        var account1 = bank.CreateDebitAccount(client1);
        account1.Deposit(10000);
        account1.PayoutRewindTime(35);

        Assert.Equal(12100, account1.Balance);
        var payout = account1.GetPayout();
        Assert.Equal(1694, payout);
    }
}