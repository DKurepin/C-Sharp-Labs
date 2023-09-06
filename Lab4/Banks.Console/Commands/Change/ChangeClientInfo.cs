using Banks.Models.Banks;
using Banks.Models.Clients;
using Spectre.Console;

namespace Banks.Console.Commands.Change;

public class ChangeClientInfo : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select what do you want to change:")
            .AddChoices(new string[]
            {
                "passport", "address", $"[red]back[/]",
            }));
        if (choice == "passport")
            SetPassport(client);
        else if (choice == "address")
            SetAddress(client);
        else if (choice == "back")
            return;
    }

    private void SetPassport(Client client)
    {
        var passport = AnsiConsole.Ask<string>("Enter passport:");
        client.SetPassport(passport);
        Success($"Passport has changed to: {passport}");
    }

    private void SetAddress(Client client)
    {
        var address = AnsiConsole.Ask<string>("Enter address:");
        client.SetAddress(address);
        Success($"Address has changed to: {address}");
    }
}