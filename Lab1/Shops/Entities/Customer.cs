using System;
using Shops.Tools;

namespace Shops.Entities;

public class Customer
{
    public const int MinMoney = 0;

    public Customer(string name, int money)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ShopException("Name of customer is incorrect. It should be letters");
        }

        Name = name;

        ArgumentNullException.ThrowIfNull(money);
        if (money <= MinMoney)
        {
            throw new ArgumentOutOfRangeException("The balance should be positive");
        }

        Balance = money;
    }

    public int Balance { get; private set; }

    public string Name { get; private set; }

    public void AddMoney(int money)
    {
        ArgumentNullException.ThrowIfNull(money);
        if (money <= MinMoney)
        {
            throw new ArgumentOutOfRangeException("You should put on your balance something positive");
        }

        Balance += money;
    }

    public void ChangeBalance(int money)
    {
        if (Balance + money < MinMoney)
        {
            throw new ShopException("You have less money on your balance than you need to buy products");
        }

        Balance += money;
    }

    public bool HasEnoughMoney(int overallCost)
    {
        if (overallCost <= MinMoney)
        {
            throw new ShopException("Overall cost of the product is less than 0");
        }

        return Balance >= overallCost;
    }
}