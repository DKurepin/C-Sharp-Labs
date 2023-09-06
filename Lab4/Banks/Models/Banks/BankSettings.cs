using Banks.Tools;

namespace Banks.Models.Banks;

public class BankSettings
{
    public BankSettings(
        decimal debitInterest,
        decimal creditLimit,
        decimal minDepositInterest,
        decimal midDepositInterest,
        decimal maxDepositInterest,
        decimal creditCommission,
        decimal suspiciousClientLimit)
    {
        CheckInterest(debitInterest);
        DebitInterest = debitInterest;
        if (creditLimit < 0)
            throw new BanksException("Credit limit must be greater than 0");
        CreditLimit = creditLimit;
        CheckBoundariesDepositInterest(minDepositInterest, midDepositInterest, maxDepositInterest);
        CheckInterest(MinDepositInterest);
        MinDepositInterest = minDepositInterest;
        CheckInterest(MidDepositInterest);
        MidDepositInterest = midDepositInterest;
        CheckInterest(MaxDepositInterest);
        MaxDepositInterest = maxDepositInterest;
        if (creditCommission < 0)
            throw new BanksException("Credit commission must be greater than 0");
        CreditCommission = creditCommission;
        if (suspiciousClientLimit < 0)
            throw new BanksException("Suspicious client limit must be greater than 0");
        SuspiciousClientLimit = suspiciousClientLimit;
    }

    public decimal SuspiciousClientLimit { get; private set; }
    public decimal DebitInterest { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal MinDepositInterest { get; private set; }
    public decimal MidDepositInterest { get; private set; }
    public decimal MaxDepositInterest { get; private set; }
    public decimal CreditCommission { get; private set; }

    public void ChangeSuspiciousClientLimit(decimal newSuspiciousClientLimit)
    {
        if (newSuspiciousClientLimit < 0)
            throw new BanksException("Suspicious client limit must be greater than 0");
        SuspiciousClientLimit = newSuspiciousClientLimit;
    }

    public void ChangeDebitInterest(decimal newDebitInterest)
    {
        CheckInterest(newDebitInterest);
        DebitInterest = newDebitInterest;
    }

    public void ChangeCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit < 0)
            throw new BanksException("Credit limit must be greater than 0");
        CreditLimit = newCreditLimit;
    }

    public void ChangeMinDepositInterest(decimal newMinimalDepositInterest)
    {
        if (newMinimalDepositInterest < 0 || newMinimalDepositInterest > 1)
            throw new BanksException("Minimal deposit percentage must be greater than 0 and less than 1");
        if (newMinimalDepositInterest > MidDepositInterest || newMinimalDepositInterest > MaxDepositInterest)
            throw new BanksException("Minimal deposit percentage must be less than mid and max deposit percentage");
        MinDepositInterest = newMinimalDepositInterest;
    }

    public void ChangeMidDepositInterest(decimal newMidDepositInterest)
    {
        if (newMidDepositInterest < 0 || newMidDepositInterest > 1)
            throw new BanksException("Mid deposit percentage must be greater than 0 and less than 1");
        if (newMidDepositInterest < MinDepositInterest || newMidDepositInterest > MaxDepositInterest)
            throw new BanksException("Mid deposit percentage must be greater than minimal and less than max deposit percentage");
        MidDepositInterest = newMidDepositInterest;
    }

    public void ChangeMaxDepositInterest(decimal newMaxDepositInterest)
    {
        if (newMaxDepositInterest < 0 || newMaxDepositInterest > 1)
            throw new BanksException("Max deposit percentage must be greater than 0 and less than 1");
        if (newMaxDepositInterest < MinDepositInterest || newMaxDepositInterest < MidDepositInterest)
            throw new BanksException("Max deposit percentage must be greater than minimal and mid deposit percentage");
        MaxDepositInterest = newMaxDepositInterest;
    }

    public void ChangeCreditCommission(decimal newCreditCommission)
    {
        if (newCreditCommission < 0)
            throw new BanksException("Credit commission must be greater than 0");
        CreditCommission = newCreditCommission;
    }

    private void CheckInterest(decimal newInterest)
    {
        if (newInterest < 0 || newInterest > 1)
            throw new BanksException("Debit percentage must be greater than 0 and less than 1");
    }

    private void CheckBoundariesDepositInterest(decimal minInterest, decimal midInterest, decimal maxInterest)
    {
        if (minInterest > midInterest || minInterest > maxInterest)
            throw new BanksException("Minimal deposit percentage must be less than mid and max deposit percentage");
        if (midInterest < minInterest || midInterest > maxInterest)
            throw new BanksException("Mid deposit percentage must be greater than minimal and less than max deposit percentage");
        if (maxInterest < minInterest || maxInterest < midInterest)
            throw new BanksException("Max deposit percentage must be greater than minimal and mid deposit percentage");
    }
}