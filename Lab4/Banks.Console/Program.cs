using Banks.Console;
using Banks.Console.Commands.Change;
using Banks.Console.Commands.Create;
using Banks.Console.Commands.Payout;
using Banks.Console.Commands.Transaction;
using Banks.Models.Banks;
using Spectre.Console;

namespace Banks.Console;

public class Program
{
    public static int Main()
    {
        var mainMenu = new ChoiceConsoleCommand(
            new Dictionary<string, ConsoleCommand>
            {
                { "create", CreateMenu() },
            }, true);
        mainMenu.BackOptionText = $"[red]exit[/]";
        mainMenu.Execute();
        AnsiConsole.Write(
            new FigletText("Plz no PPA")
                .Centered()
                .Color(Color.Green));
        return 0;
    }

    private static ChoiceConsoleCommand CreateMenu()
    {
        var command = new ChoiceConsoleCommand(
            new Dictionary<string, ConsoleCommand>
            {
                { "account", new CreateAccountCommand() },
                { "bank", BankMenu() },
                { "client", ClientMenu() },
                { "transaction", TransactionMenu() },
                { "payout time rewind", new NewPayoutTimeRewind() },
            }, true);
        return command;
    }

    private static ChoiceConsoleCommand TransactionMenu()
    {
        var command = new ChoiceConsoleCommand(
            new Dictionary<string, ConsoleCommand>
            {
                { "1. create", new NewTransaction() },
                { "2. history", new TransactionHistory() },
            }, true);
        return command;
    }

    private static ChoiceConsoleCommand BankMenu()
    {
        var command = new ChoiceConsoleCommand(
            new Dictionary<string, ConsoleCommand>
            {
                { "1. create bank", new CreateBankCommand() },
                { "2. bank info", new BankInfo() },
                { "3. change bank settings", new ChangeBankSettings() },
            }, true);
        return command;
    }

    private static ChoiceConsoleCommand ClientMenu()
    {
        var command = new ChoiceConsoleCommand(
            new Dictionary<string, ConsoleCommand>
            {
                { "1. create client", new CreateClientCommand() },
                { "2. client info", new ClientInfo() },
                { "3. change client info", new ChangeClientInfo() },
            }, true);
        return command;
    }
}