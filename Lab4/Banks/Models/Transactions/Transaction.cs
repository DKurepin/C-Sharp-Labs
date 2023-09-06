using Banks.Models.Accounts;
using Banks.Tools;

namespace Banks.Models.Transactions;

public abstract class Transaction
{
    public Transaction(Account account, decimal paymentAmount, DateTime time)
    {
        Id = Guid.NewGuid();
        Account = account ?? throw new BanksException("Sender cannot be null");
        if (paymentAmount < 0)
            throw new BanksException("Payment amount must be greater than 0");
        PaymentAmount = paymentAmount;
        Time = time;
        IsCanceled = false;
    }

    public bool IsCanceled { get; protected set; }
    public bool IsSuccessful { get; protected set; }
    public Guid Id { get; }
    public Account Account { get; }
    public decimal PaymentAmount { get; }
    public DateTime Time { get; }

    public abstract void Cancel();
}