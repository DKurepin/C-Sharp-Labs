using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Accounts;

public abstract class Account
{
    private List<Transaction> _transactions = new List<Transaction>();

    public Account(Client client, Bank bank)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(bank, nameof(bank));
        AccountHolder = client;
        ConcreteBank = bank;
        Id = Guid.NewGuid();
    }

    public bool Verification => AccountHolder.IsVerified;
    public DateTime CurrentTime => CentralBank.GetInstance().TimeManager.CentralBankTime;
    public Guid Id { get; }
    public Bank ConcreteBank { get; }
    public Client AccountHolder { get; }
    public decimal Balance { get; protected set; } = 0;

    public IReadOnlyList<Transaction> Transactions => _transactions;

    public void Deposit(decimal amount)
    {
        if (amount < 0)
            throw new BanksException("Amount must be greater than 0");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0)
            throw new BanksException("Amount must be greater than 0");
        Balance -= amount;
    }

    public virtual void PayoutRewindTime(int days)
    {
    }

    public virtual decimal GetPayout()
    {
        return 0;
    }

    public void AddTransaction(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        _transactions.Add(transaction);
    }

    public void TransactionCheck(Transaction transaction)
    {
        if (!_transactions.Contains(transaction))
            throw new BanksException("Transaction not found");
    }

    public abstract void CheckWithdraw(decimal amount);
}