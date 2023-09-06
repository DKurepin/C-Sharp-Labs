using Banks.Models.Accounts;
using Banks.Tools;

namespace Banks.Models.Transactions;

public class WithdrawTransaction : Transaction
{
    public WithdrawTransaction(Account account, decimal paymentAmount, DateTime time)
        : base(account, paymentAmount, time)
    {
        Account.CheckWithdraw(paymentAmount);
        Account.Withdraw(paymentAmount);
        Account.AddTransaction(this);
        IsSuccessful = true;
    }

    public override void Cancel()
    {
        if (IsCanceled)
            throw new BanksException("Transaction is already canceled");
        if (!IsSuccessful)
            throw new BanksException("Transaction is not successful");
        Account.Deposit(PaymentAmount);
        IsCanceled = true;
    }
}