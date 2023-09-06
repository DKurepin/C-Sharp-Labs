using System.ComponentModel;
using System.Data.Common;
using System.Transactions;
using Banks.Models.Accounts;
using Banks.Models.Clients;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Models.Banks;

public class Bank : IObservable
{
    private List<Account> _accounts;
    private List<Client> _clients;
    private List<Transaction> _transactions;

    public Bank(string name, BankSettings settings)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Bank name cannot be empty");
        if (settings is null)
            throw new BanksException("Bank settings cannot be null");

        Name = name;
        Settings = settings;
        Id = Guid.NewGuid();
        _accounts = new List<Account>();
        _clients = new List<Client>();
        _transactions = new List<Transaction>();
    }

    public string Name { get; private set; }
    public BankSettings Settings { get; private set; }
    public Guid Id { get; }

    public IReadOnlyList<Account> Accounts => _accounts;
    public IReadOnlyList<Client> Clients => _clients;

    public IReadOnlyList<Transaction> Transactions => _transactions;

    public void AddClient(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        if (_clients.Contains(client))
            throw new BanksException("Client already exists");
        _clients.Add(client);
    }

    public CreditAccount CreateCreditAccount(Client client)
    {
        if (!BankHasClient(client))
            throw new BanksException("Client not found");
        if (client is null)
            throw new BanksException("Client cannot be null");
        var account = new CreditAccount(client, this);
        _accounts.Add(account);
        return account;
    }

    public DepositAccount CreateDepositAccount(Client client, DateTime closingDate)
    {
        if (!BankHasClient(client))
            throw new BanksException("Client not found");
        if (closingDate < DateTime.Now)
            throw new BanksException("Closing date cannot be in the past");
        if (client is null)
            throw new BanksException("Client cannot be null");
        var account = new DepositAccount(client, this, closingDate);
        _accounts.Add(account);
        return account;
    }

    public DebitAccount CreateDebitAccount(Client client)
    {
        if (!BankHasClient(client))
            throw new BanksException("Client not found");
        if (client is null)
            throw new BanksException("Client cannot be null");
        var account = new DebitAccount(client, this);
        _accounts.Add(account);
        return account;
    }

    public void AddObserver(Client client)
    {
        if (client is null)
            throw new BanksException("Observer cannot be null");
        if (BankHasClient(client))
            client.ChangeSubscription(true);
    }

    public void RemoveObserver(Client client)
    {
        if (client is null)
            throw new BanksException("Observer cannot be null");
        if (BankHasClient(client))
            client.ChangeSubscription(false);
    }

    public void Notify(string message)
    {
        var subscribedClients = _clients.Where(client => client.IsSubscribed).ToList();
        foreach (Client client in subscribedClients)
        {
            var update = $"Bank {Name} notifies you: {message}";
            client.Update(update);
        }
    }

    public bool BankHasClient(Client client)
    {
        if (client is null)
            throw new BanksException("Client cannot be null");
        return _clients.Contains(client);
    }

    public void ChangeSuspiciousClientLimit(decimal newSuspiciousClientLimit)
    {
        Settings.ChangeSuspiciousClientLimit(newSuspiciousClientLimit);
        Notify($"Suspicious client limit changed to {newSuspiciousClientLimit}");
    }

    public void ChangeDebitInterest(decimal newDebitInterest)
    {
        Settings.ChangeDebitInterest(newDebitInterest);
        Notify($"Debit interest changed to {newDebitInterest}");
    }

    public void ChangeCreditLimit(decimal newCreditLimit)
    {
        Settings.ChangeCreditLimit(newCreditLimit);
        Notify($"Credit limit changed to {newCreditLimit}");
    }

    public void ChangeMinDepositInterest(decimal newMinimalDepositInterest)
    {
        Settings.ChangeMinDepositInterest(newMinimalDepositInterest);
        Notify($"Minimal deposit interest changed to {newMinimalDepositInterest}");
    }

    public void ChangeMidDepositInterest(decimal newMidDepositInterest)
    {
        Settings.ChangeMidDepositInterest(newMidDepositInterest);
        Notify($"Middle deposit interest changed to {newMidDepositInterest}");
    }

    public void ChangeMaxDepositInterest(decimal newMaxDepositInterest)
    {
        Settings.ChangeMaxDepositInterest(newMaxDepositInterest);
        Notify($"Maximal deposit interest changed to {newMaxDepositInterest}");
    }

    public void ChangeCreditCommission(decimal newCreditCommission)
    {
        Settings.ChangeCreditCommission(newCreditCommission);
        Notify($"Credit commission changed to {newCreditCommission}");
    }
}