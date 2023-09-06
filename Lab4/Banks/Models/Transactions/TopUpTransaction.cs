using Banks.Models.Accounts;
using Banks.Tools;

namespace Banks.Models.Transactions;

public class TopUpTransaction : Transaction
{
    public TopUpTransaction(Account account, decimal paymentAmount, DateTime time)
        : base(account, paymentAmount, time)
    {
        if (paymentAmount <= 0)
            throw new BanksException("Payment amount must be greater than 0");
        Account.Deposit(paymentAmount);
        Account.AddTransaction(this);
        IsSuccessful = true;
    }

    public override void Cancel()
    {
        if (IsCanceled)
            throw new BanksException("Transaction is already canceled");
        if (!IsSuccessful)
            throw new BanksException("Transaction is not successful");
        Account.Withdraw(PaymentAmount);
        IsCanceled = true;
    }
}