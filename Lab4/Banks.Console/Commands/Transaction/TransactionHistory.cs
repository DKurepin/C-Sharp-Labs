using Spectre.Console;

namespace Banks.Console.Commands.Transaction;

public class TransactionHistory : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);
        var account = SelectAccount(bank, client);
        var table = new Table();
        table.AddColumns(new[]
        {
            "Date", "Name", "Amount", "Transaction Id",
        });
        foreach (var transaction in account.Transactions.Where(t => !t.IsCanceled && t.IsSuccessful))
        {
            var date = transaction.Time.Date.ToString();
            table.AddRow(new string[]
            {
                date, transaction.GetType().Name, transaction.PaymentAmount.ToString(), transaction.Id.ToString(),
            });
        }

        AnsiConsole.Write(table);
        AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue:").AllowEmpty().Secret());
    }
}