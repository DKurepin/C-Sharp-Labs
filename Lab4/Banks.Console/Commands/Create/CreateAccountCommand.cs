using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Tools;
using Spectre.Console;

namespace Banks.Console.Commands.Create;

public class CreateAccountCommand : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select account type:")
            .AddChoices(new string[]
            {
                "credit", "debit", "deposit", $"[red]back[/]",
            }));

        if (!bank.BankHasClient(client))
            bank.AddClient(client);

        if (choice == "credit")
            MakeCreditAccount(bank, client);
        else if (choice == "debit")
            MakeDebitAccount(bank, client);
        else if (choice == "deposit")
            MakeDepositAccount(bank, client);
        else if (choice == "back")
            return;
    }

    private void MakeCreditAccount(Bank bank, Client client)
    {
        var account = bank.CreateCreditAccount(client);
        Success($"Account: {account.Id} created");
    }

    private void MakeDebitAccount(Bank bank, Client client)
    {
        var account = bank.CreateDebitAccount(client);
        Success($"Account: {account.Id} created");
    }

    private void MakeDepositAccount(Bank bank, Client client)
    {
        var days = AnsiConsole.Ask<int>("Deposit interval in days:");
        var endDate = CentralBank.GetInstance().TimeManager.CentralBankTime.AddDays(days);
        var account = bank.CreateDepositAccount(client, endDate);
        Success($"Account: {account.Id} created");
    }
}