using Banks.Tools;

namespace Banks.Models.Accounts;

public class TimeManager
{
    public TimeManager()
    {
        CentralBankTime = DateTime.Today;
    }

    public DateTime CentralBankTime { get; private set; }

    public void RewindTimeDays(int days)
    {
        if (days < 0)
            throw new BanksException("Days cannot be negative");
        CentralBankTime = CentralBankTime.AddDays(days);
    }

    public bool IsItLastDayOfMonth()
    {
        var currentDate = CentralBankTime;
        var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        return currentDate == lastDayOfMonth;
    }
}