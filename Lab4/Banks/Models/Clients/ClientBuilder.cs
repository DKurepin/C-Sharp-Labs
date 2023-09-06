using Banks.Tools;

namespace Banks.Models.Clients;

public class ClientBuilder
{
    private string _name = null!;
    private string _surname = null!;
    private string _address = null!;
    private string _passport = null!;

    public ClientBuilder SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Name is null or empty");
        _name = name;
        return this;
    }

    public ClientBuilder SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new BanksException("Surname is null or empty");
        _surname = surname;
        return this;
    }

    public ClientBuilder SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BanksException("Address is null or empty");
        _address = address;
        return this;
    }

    public ClientBuilder SetPassport(string passport)
    {
        if (string.IsNullOrWhiteSpace(passport))
            throw new BanksException("Passport is null or empty");
        _passport = passport;
        return this;
    }

    public Client CreateClient()
    {
        return new Client(_name, _surname, _address, _passport);
    }
}