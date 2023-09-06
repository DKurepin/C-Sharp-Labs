using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools;

namespace Banks.Models.Accounts;

public class DepositAccount : Account
{
    private decimal _payout;

    public DepositAccount(Client client, Bank bank, DateTime closingDate)
        : base(client, bank)
    {
        if (closingDate < DateTime.Now)
            throw new BanksException("Closing date must be greater than current date");
        if (closingDate - CurrentTime < TimeSpan.FromDays(30))
            throw new BanksException("Closing date must be greater than 30 days");
        ClosingDate = closingDate;
        IsAccountExpired = false;
    }

    public bool IsAccountExpired { get; private set; }
    public DateTime ClosingDate { get; }

    public override void PayoutRewindTime(int days)
    {
        if (days < 0)
            throw new BanksException("Days can't be less than 0");
        int paymentDays = days;
        var currentInterest = CheckBalance(Balance);
        var endPaymentDate = CentralBank.GetInstance().TimeManager.CentralBankTime.AddDays(days);
        if (CentralBank.GetInstance().TimeManager.CentralBankTime.Month == endPaymentDate.Month &&
            ClosingDate > endPaymentDate)
        {
            for (int day = 0; day < paymentDays; day++)
            {
                _payout = (Balance * currentInterest) * days;
                Deposit(_payout);
                _payout = 0;
            }
        }
        else
        {
            for (int day = 0; day < paymentDays; day++)
            {
                _payout += Balance * currentInterest;
                if (CentralBank.GetInstance().TimeManager.IsItLastDayOfMonth())
                {
                    Deposit(_payout);
                    _payout = 0;
                }

                if (CentralBank.GetInstance().TimeManager.CentralBankTime.Date == ClosingDate)
                {
                    Deposit(_payout);
                    IsAccountExpired = true;
                    _payout = 0;
                }

                CentralBank.GetInstance().TimeManager.RewindTimeDays(1);
            }
        }
    }

    public override decimal GetPayout()
    {
        return _payout;
    }

    public override void CheckWithdraw(decimal amount)
    {
        if (Balance < amount)
            throw new BanksException("Not enough money");
        if (CentralBank.GetInstance().TimeManager.CentralBankTime < ClosingDate)
            throw new BanksException("Deposit account is not closed yet");
        if (!AccountHolder.IsVerified && amount > ConcreteBank.Settings.SuspiciousClientLimit)
            throw new BanksException($"Unverified client can't withdraw more than {ConcreteBank.Settings.SuspiciousClientLimit}");
    }

    private decimal CheckBalance(decimal amount)
    {
        if (amount < 50000)
            return ConcreteBank.Settings.MinDepositInterest;
        else if (amount >= 50000 && amount < 100000)
            return ConcreteBank.Settings.MidDepositInterest;
        else
            return ConcreteBank.Settings.MaxDepositInterest;
    }
}