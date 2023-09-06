using Banks.Models.Clients;

namespace Banks.Observer;

public interface IObservable
{
    void AddObserver(Client client);
    void RemoveObserver(Client client);
    void Notify(string message);
}