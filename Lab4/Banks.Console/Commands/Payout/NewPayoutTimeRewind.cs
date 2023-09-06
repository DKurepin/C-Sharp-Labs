using Banks.Models.Accounts;
using Banks.Models.Banks;
using Spectre.Console;

namespace Banks.Console.Commands.Payout;

public class NewPayoutTimeRewind : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);
        var account = SelectAccount(bank, client);
        if (account is CreditAccount)
        {
            Failure("You can't rewind time on credit account");
            return;
        }

        var days = AnsiConsole.Ask<int>("Write how many days you want to rewind:");
        account.PayoutRewindTime(days);
        var payout = account.GetPayout();
        Success($"Rewinded {days} days \n Balance: {account.Balance} \n Payout: {payout}");
    }
}