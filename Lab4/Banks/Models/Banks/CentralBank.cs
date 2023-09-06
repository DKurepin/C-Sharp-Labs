using Banks.Models.Accounts;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.Banks;

public class CentralBank
{
    private static CentralBank? _instance = null;
    private List<Bank> _banks;
    private List<Transaction> _transactionsHistory;

    private CentralBank()
    {
        _banks = new List<Bank>();
        _transactionsHistory = new List<Transaction>();
        TimeManager = new TimeManager();
    }

    public TimeManager TimeManager { get; }

    public bool IsCentralBankCreated => _instance != null;
    public IReadOnlyList<Bank> Banks => _banks;

    public static CentralBank GetInstance()
    {
        if (_instance is null)
            _instance = new CentralBank();
        return _instance;
    }

    public bool BankExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Bank name cannot be empty");
        return _banks.Any(bank => bank?.Name == name);
    }

    public Bank CreateBank(string name, BankSettings settings)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Bank name cannot be empty");
        if (settings is null)
            throw new BanksException("Bank settings cannot be null");
        if (BankExists(name))
            throw new BanksException("Bank already exists");
        Bank bank = new Bank(name, settings);
        _banks.Add(bank);
        return bank;
    }
}