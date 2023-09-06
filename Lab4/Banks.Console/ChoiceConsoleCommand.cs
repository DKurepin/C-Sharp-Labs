using Spectre.Console;

namespace Banks.Console;

public class ChoiceConsoleCommand : ConsoleCommand
{
    private readonly bool loop = false;
    private Dictionary<string, ConsoleCommand> options;

    public ChoiceConsoleCommand(Dictionary<string, ConsoleCommand> options, bool loop = false)
    {
        ArgumentNullException.ThrowIfNull(options);
        this.options = options;
        this.loop = loop;
    }

    public string BackOptionText { get; set; } = $"[red]back[/]";

    public override void Execute()
    {
        var opts = options.Keys.ToList();
        opts.Sort();
        opts.Add(BackOptionText);

        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select option:")
                .AddChoices(opts));
            if (choice == BackOptionText)
                return;
            try
            {
                options[choice].Execute();
            }
            catch (Exception exception)
            {
                Failure(exception.Message);
            }

            if (!loop)
                return;
        }
    }
}