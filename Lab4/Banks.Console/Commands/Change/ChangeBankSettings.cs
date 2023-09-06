using Banks.Models.Banks;
using Spectre.Console;

namespace Banks.Console.Commands.Change;

public class ChangeBankSettings : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select what do you want to change:")
            .AddChoices(new string[]
            {
                "debit interest",
                "credit limit",
                "min deposit interest",
                "mid deposit interest",
                "max deposit interest",
                "credit commission",
                "suspicious client limit",
                $"[red]back[/]",
            }));
        if (choice == "debit interest")
            SetDebitInterest(bank);
        else if (choice == "credit limit")
            SetCreditLimit(bank);
        else if (choice == "min deposit interest")
            SetMinDepositInterest(bank);
        else if (choice == "mid deposit interest")
            SetMidDepositInterest(bank);
        else if (choice == "max deposit interest")
            SetMaxDepositInterest(bank);
        else if (choice == "credit commission")
            SetCreditCommission(bank);
        else if (choice == "suspicious client limit")
            SetSuspiciousClientLimit(bank);
        else if (choice == "back")
            return;
    }

    private void SetSuspiciousClientLimit(Bank bank)
    {
        var limit = AnsiConsole.Ask<decimal>("Enter new suspicious client limit:");
        bank.ChangeSuspiciousClientLimit(limit);
        Success($"Suspicious client limit has changed to: {limit}");
    }

    private void SetCreditCommission(Bank bank)
    {
        var commission = AnsiConsole.Ask<decimal>("Enter new credit commission:");
        bank.ChangeCreditCommission(commission);
        Success($"Credit commission has changed to: {commission}");
    }

    private void SetMaxDepositInterest(Bank bank)
    {
        var interest = AnsiConsole.Ask<decimal>("Enter new max deposit interest:");
        bank.ChangeMaxDepositInterest(interest);
        Success($"Max deposit interest has changed to: {interest}");
    }

    private void SetMidDepositInterest(Bank bank)
    {
        var interest = AnsiConsole.Ask<decimal>("Enter new mid deposit interest:");
        bank.ChangeMidDepositInterest(interest);
        Success($"Mid deposit interest has changed to: {interest}");
    }

    private void SetMinDepositInterest(Bank bank)
    {
        var interest = AnsiConsole.Ask<decimal>("Enter new min deposit interest:");
        bank.ChangeMinDepositInterest(interest);
        Success($"Min deposit interest has changed to: {interest}");
    }

    private void SetCreditLimit(Bank bank)
    {
        var limit = AnsiConsole.Ask<decimal>("Enter new credit limit:");
        bank.ChangeCreditLimit(limit);
        Success($"Credit limit has changed to: {limit}");
    }

    private void SetDebitInterest(Bank bank)
    {
        var interest = AnsiConsole.Ask<decimal>("Enter new debit interest:");
        bank.ChangeDebitInterest(interest);
        Success($"Debit interest has changed to: {interest}");
    }
}