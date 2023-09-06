using Banks.Models.Banks;
using Spectre.Console;

namespace Banks.Console.Commands.Create;

public class CreateBankCommand : ConsoleCommand
{
    public override void Execute()
    {
        var name = AnsiConsole.Ask<string>("Name:");
        BankSettings defaultBankSettings = new BankSettings(0.01M, 10000M, 0.03M, 0.035M, 0.04M, 0.01M, 5000M);
        var bank = CentralBank.GetInstance().CreateBank(name, defaultBankSettings);
        Success($"Bank: {name} created with default settings");
    }
}