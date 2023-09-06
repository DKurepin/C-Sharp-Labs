using Banks.Models.Accounts;
using Banks.Tools;

namespace Banks.Models.Transactions;

public class TransferTransaction : Transaction
{
    public TransferTransaction(Account sender, Account receiver, decimal paymentAmount, DateTime time)
        : base(sender, paymentAmount, time)
    {
        Receiver = receiver ?? throw new BanksException("Receiver cannot be null");
        if (sender == receiver)
        {
            throw new BanksException("Sender and receiver cannot be the same");
        }
        else
        {
            Account.CheckWithdraw(paymentAmount);
            Account.Withdraw(paymentAmount);
            Account.AddTransaction(this);

            Receiver.Deposit(paymentAmount);
            Receiver.AddTransaction(this);
            IsSuccessful = true;
        }
    }

    public Account Receiver { get; }

    public override void Cancel()
    {
        if (IsCanceled)
            throw new BanksException("Transaction is already canceled");
        if (!IsSuccessful)
            throw new BanksException("Transaction is not successful");
        Account.Deposit(PaymentAmount);
        Receiver.CheckWithdraw(PaymentAmount);
        Receiver.Withdraw(PaymentAmount);
        IsCanceled = true;
    }
}