using Shops.Tools;

namespace Shops.Entities;

public class Product
{
    public const int MinPrice = 0;
    public const int MinAmount = 0;
    public Product(string name, int price, int amount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ShopException("Name of product is null or empty");
        }

        Name = name;

        ArgumentNullException.ThrowIfNull(price);
        if (price <= MinPrice)
        {
            throw new ShopException("Price should be positive");
        }

        Price = price;

        ArgumentNullException.ThrowIfNull(amount);
        if (amount <= MinAmount)
        {
            throw new ShopException("Amount should be positive");
        }

        Amount = amount;
    }

    public int Amount { get; private set; }

    public int Price { get; private set; }

    public string Name { get; }

    internal void SetAmount(int amount)
    {
        if (amount > Amount)
        {
            throw new ShopException("Amount should be positive");
        }

        Amount -= amount;
    }

    internal void SetPrice(int newPrice)
    {
        if (newPrice <= MinPrice)
        {
            throw new ShopException("Price should be more than 0");
        }

        Price = newPrice;
    }
}