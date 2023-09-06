using Banks.Observer;
using Banks.Tools;

namespace Banks.Models.Clients;

public class Client : IObserver
{
    private List<string> _notifications;

    public Client(string name, string surname, string address, string passport)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Name is null or empty");
        if (string.IsNullOrWhiteSpace(surname))
            throw new BanksException("Surname is null or empty");

        Name = name;
        Surname = surname;
        Address = address;
        Passport = passport;
        IsSubscribed = false;
        _notifications = new List<string>();
    }

    public IReadOnlyList<string> Notifications => _notifications;

    public string Passport { get; private set; }
    public string Address { get; private set; }
    public string Surname { get; private set; }
    public string Name { get; private set; }

    public bool IsVerified => !string.IsNullOrWhiteSpace(Address) || !string.IsNullOrWhiteSpace(Passport);
    public bool IsSubscribed { get; private set; }

    public void SetPassport(string passport)
    {
        if (string.IsNullOrWhiteSpace(passport))
            throw new BanksException("Passport is null or empty");
        Passport = passport;
    }

    public void SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BanksException("Address is null or empty");
        Address = address;
    }

    public void Update(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new BanksException("Message is null or empty");
        _notifications.Add(message);
    }

    public void ChangeSubscription(bool isSubscribed)
    {
        IsSubscribed = isSubscribed;
    }
}