using Spectre.Console;

namespace Banks.Console.Commands.Create;

public class ClientInfo : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);
        var table1 = new Table();
        table1.AddColumns(new[]
        {
            "Full name", "Passport", "Address", "Verified client",
        }).Centered();
        var verified = client.IsVerified ? "Yes" : "No";
        var noPassport = string.IsNullOrWhiteSpace(client.Passport) ? "-" : client.Passport;
        var noAddress = string.IsNullOrWhiteSpace(client.Address) ? "-" : client.Address;
        table1.AddRow(new string[]
        {
            $"{client.Name} {client.Surname}", noPassport, noAddress, verified,
        }).Centered();

        var table2 = new Table();
        table2.AddColumns(new[]
        {
            "Bank", "Type", "Account", "Balance", "Payout",
        }).Centered();
        var clientAccounts = bank.Accounts.Where(a => a.AccountHolder == client);
        foreach (var account in clientAccounts)
        {
            table2.AddRow(new string[]
            {
                account.ConcreteBank.Name,
                account.GetType().Name,
                account.Id.ToString(),
                account.Balance.ToString(),
                account.GetPayout().ToString(),
            }).Centered();
        }

        AnsiConsole.Write(table1);
        AnsiConsole.Write(table2);
        AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue:").AllowEmpty().Secret());
    }
}