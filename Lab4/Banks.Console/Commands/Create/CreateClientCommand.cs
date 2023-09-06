using Banks.Models.Clients;
using Spectre.Console;

namespace Banks.Console.Commands.Create;

public class CreateClientCommand : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();

        if (bank is null)
        {
            Failure("No banks available");
            return;
        }

        var builder = new ClientBuilder();
        builder.SetName(AnsiConsole.Ask<string>("Name:"));
        builder.SetSurname(AnsiConsole.Ask<string>("Surname:"));

        var temp = AnsiConsole.Prompt(new TextPrompt<string>("Passport:").AllowEmpty());
        if (!string.IsNullOrWhiteSpace(temp))
            builder.SetPassport(temp);

        temp = AnsiConsole.Prompt(new TextPrompt<string>("Address:").AllowEmpty());
        if (!string.IsNullOrWhiteSpace(temp))
            builder.SetAddress(temp);

        var client = builder.CreateClient();
        bank.AddClient(client);
        Success($"Created client: {client.Name} {client.Surname}");
    }
}