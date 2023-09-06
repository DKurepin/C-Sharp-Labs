namespace Isu.Extra.Tools;

public class IsuExtraException : Exception
{
    public IsuExtraException()
    {
    }

    public IsuExtraException(string message)
        : base(message)
    {
    }

    public IsuExtraException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
