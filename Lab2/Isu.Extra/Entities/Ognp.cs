using Isu.Extra.Tools;
namespace Isu.Extra.Entities;

public class Ognp
{
    private readonly List<Flow> _flows = new List<Flow>();

    public Ognp(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuExtraException("Name is incorrect");

        if (name[0] < 'A' || name[0] > 'Z')
            throw new IsuExtraException("There is no letter code for the faculty");

        Name = name;
    }

    public IReadOnlyList<Flow> Flows => _flows;

    public string Name { get; }

    public char FacultyCode => Name[0];

    public Flow AddFlow(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new IsuExtraException("Invalid name");

        if (_flows.Any(flow => flow.Name == name))
            throw new IsuExtraException("This name already exists");

        Flow flow = new Flow(this, name);
        _flows.Add(flow);
        return flow;
    }
}