namespace Banks.Console;

public class BanksConsoleException : Exception
{
    public BanksConsoleException()
    {
    }

    public BanksConsoleException(string message)
        : base(message)
    {
    }

    public BanksConsoleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}