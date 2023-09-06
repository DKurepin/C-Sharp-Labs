using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools;

namespace Banks.Models.Accounts;

public class CreditAccount : Account
{
    public CreditAccount(Client client, Bank bank)
        : base(client, bank)
    {
    }

    public decimal Commission => ConcreteBank.Settings.CreditCommission;

    public override void CheckWithdraw(decimal amount)
    {
        if (!AccountHolder.IsVerified && amount > ConcreteBank.Settings.SuspiciousClientLimit)
            throw new BanksException($"Unverified client can't withdraw more than {ConcreteBank.Settings.SuspiciousClientLimit}");
        if (Balance - amount < -ConcreteBank.Settings.CreditLimit)
            throw new BanksException($"Credit limit is {ConcreteBank.Settings.CreditLimit}");
        if (Balance < amount)
            Balance -= Commission;
    }
}