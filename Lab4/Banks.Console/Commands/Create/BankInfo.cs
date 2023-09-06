using Spectre.Console;

namespace Banks.Console.Commands.Create;

public class BankInfo : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var table = new Table();
        table.AddColumns(new[]
        {
            "Debit interest",
            "Credit limit",
            "Min deposit interest",
            "Mid deposit interest",
            "Max deposit interest",
            "Credit commission",
            "Suspicious client limit",
        }).Centered();

        table.AddRow(new string[]
        {
            bank.Settings.DebitInterest.ToString(),
            bank.Settings.CreditLimit.ToString(),
            bank.Settings.MinDepositInterest.ToString(),
            bank.Settings.MidDepositInterest.ToString(),
            bank.Settings.MaxDepositInterest.ToString(),
            bank.Settings.CreditCommission.ToString(),
            bank.Settings.SuspiciousClientLimit.ToString(),
        }).Centered();

        table.Title($"{bank.Name} settings");
        AnsiConsole.Write(table);
        AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue:").AllowEmpty().Secret());
    }
}