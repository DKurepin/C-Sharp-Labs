using System.ComponentModel.Design;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools;
using Microsoft.VisualBasic;

namespace Banks.Models.Accounts;

public class DebitAccount : Account
{
    private decimal _payout = 0;

    public DebitAccount(Client client, Bank bank)
        : base(client, bank)
    {
    }

    public override void PayoutRewindTime(int days)
    {
        if (days < 0)
            throw new BanksException("Days can't be less than 0");
        int paymentDays = days;
        var endPaymentDate = CentralBank.GetInstance().TimeManager.CentralBankTime.AddDays(days);
        if (CentralBank.GetInstance().TimeManager.CentralBankTime.Month == endPaymentDate.Month)
        {
            _payout = (Balance * ConcreteBank.Settings.DebitInterest) * days;
            Deposit(_payout);
            _payout = 0;
        }
        else
        {
            for (int day = 0; day < paymentDays; day++)
            {
                _payout += Balance * ConcreteBank.Settings.DebitInterest;
                if (CentralBank.GetInstance().TimeManager.IsItLastDayOfMonth())
                {
                    Deposit(_payout);
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
        if (!AccountHolder.IsVerified && amount > ConcreteBank.Settings.SuspiciousClientLimit)
            throw new BanksException($"Unverified client can't withdraw more than {ConcreteBank.Settings.SuspiciousClientLimit}");
    }
}