using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Spectre.Console;

namespace Banks.Console;

public abstract class ConsoleCommand
{
    public abstract void Execute();

    protected Bank SelectBank()
    {
        AnsiConsole.Clear();
        var banks = new Dictionary<string, Bank>();
        foreach (var bank in CentralBank.GetInstance().Banks)
            banks[bank.Name] = bank;

        if (banks.Count == 0)
            throw new BanksConsoleException("No available banks");

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select bank:")
            .AddChoices(banks.Keys));
        return banks[choice];
    }

    protected Client SelectClient(Bank bank)
    {
        AnsiConsole.Clear();
        var clients = new Dictionary<string, Client>();
        foreach (var client in bank.Clients)
            clients[$"{client.Name} {client.Surname}"] = client;
        if (clients.Count == 0)
            throw new BanksConsoleException($"No available clients in {bank.Name}");

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select client:")
            .AddChoices(clients.Keys));
        return clients[choice];
    }

    protected Account SelectAccount(Bank bank, Client client)
    {
        AnsiConsole.Clear();
        var accounts = new Dictionary<string, Account>();
        foreach (var account in bank.Accounts.Where(a => a.AccountHolder == client))
        {
            var key = $"{account.GetType().Name}\t{account.Id}";
            accounts[key] = account;
        }

        if (accounts.Count == 0)
            throw new BanksConsoleException($"Client: {client.Name} {client.Surname} has no accounts in {bank.Name}");

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select account:")
            .AddChoices(accounts.Keys));
        return accounts[choice];
    }

    protected Transaction SelectTransaction(Bank bank, Account account)
    {
        AnsiConsole.Clear();
        var transactions = new Dictionary<string, Transaction>();
        foreach (var transaction in account.Transactions.Where(t => !t.IsCanceled && t.IsSuccessful))
        {
            var date = transaction.Time.Date.ToString();
            var key = $"{date}\t{transaction.GetType().Name}\t{transaction.PaymentAmount}\t{transaction.Id}";
            transactions[key] = transaction;
        }

        if (transactions.Count == 0)
            throw new BanksConsoleException($"Account: {account.Id} has no transactions");

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select transaction:")
            .AddChoices(transactions.Keys));
        return transactions[choice];
    }

    protected void Success(string message, int sleepMiliseconds = 2000)
    {
        AnsiConsole.Markup($"[green]{message}[/]");
        Thread.Sleep(sleepMiliseconds);
    }

    protected void Failure(string message, int sleepMiliseconds = 2000)
    {
        AnsiConsole.Markup($"[red]{message}[/]");
        Thread.Sleep(sleepMiliseconds);
    }
}