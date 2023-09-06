using Banks.Models.Accounts;
using Banks.Models.Banks;
using Banks.Models.Transactions;
using Spectre.Console;

namespace Banks.Console.Commands.Transaction;

public class NewTransaction : ConsoleCommand
{
    public override void Execute()
    {
        var bank = SelectBank();
        var client = SelectClient(bank);

        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select option:")
            .AddChoices(new[]
            {
                "top up", "transfer", "withdraw", "cancel", $"[red]back[/]",
            }));

        if (choice == "top up")
        {
            var account = SelectAccount(bank, client);
            CreateTopUpTransaction(account);
        }
        else if (choice == "withdraw")
        {
            var account = SelectAccount(bank, client);
            CreateWithdrawTransaction(account);
        }
        else if (choice == "transfer")
        {
            var sender = SelectAccount(bank, client);
            var bankReciever = SelectBank();
            var clientReceiver = SelectClient(bankReciever);
            var receiver = SelectAccount(bankReciever, clientReceiver);
            CreateTransferTransaction(sender, receiver);
        }
        else if (choice == "cancel")
        {
            var account = SelectAccount(bank, client);
            CancelTransaction(bank, account);
        }
        else if (choice == "back")
        {
            return;
        }
    }

    private void CreateTopUpTransaction(Account account)
    {
        var amount = AnsiConsole.Ask<decimal>("Amount:");
        var transaction = new TopUpTransaction(account, amount, CentralBank.GetInstance().TimeManager.CentralBankTime);
        Success($"Transaction created! Account: {account.Id} balance: {account.Balance}");
    }

    private void CreateWithdrawTransaction(Account account)
    {
        var amount = AnsiConsole.Ask<decimal>("Amount:");
        var transaction =
            new WithdrawTransaction(account, amount, CentralBank.GetInstance().TimeManager.CentralBankTime);
        Success($"Transaction created! Account: {account.Id} balance: {account.Balance}");
    }

    private void CancelTransaction(Bank bank, Account account)
    {
        var transactionToCancel = SelectTransaction(bank, account);
        transactionToCancel.Cancel();
        Success($"Transaction canceled! Account: {account.Id} balance: {account.Balance}");
    }

    private void CreateTransferTransaction(Account sender, Account receiver)
    {
        var amount = AnsiConsole.Ask<decimal>("Amount:");
        var transaction =
            new TransferTransaction(sender, receiver, amount, CentralBank.GetInstance().TimeManager.CentralBankTime);
        Success(
            $"Transaction created! Account: {sender.Id} balance: {sender.Balance} \n Account: {receiver.Id} balance: {receiver.Balance}");
    }
}