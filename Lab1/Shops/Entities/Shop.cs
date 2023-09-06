using System;
using Shops.Tools;

namespace Shops.Entities;

public class Shop
{
    public Shop(string name, string address, int id)
    {
        ID = id;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ShopException("Name of shop should is incorrect. It should be letters");
        }

        Name = name;

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ShopException("Address of shop should is incorrect. It should be letters");
        }

        Address = address;
        Products = new List<Product>();
    }

    public List<Product> Products { get; private set; }

    public string Address { get; private set; }

    public string Name { get; private set; }

    public int ID { get; private set; }

    public void BuyProduct(string nameOfProduct, int amount)
    {
        Product? product = Products.Find(p => p.Name == nameOfProduct);
        if (product is null)
            throw new ShopException("There is no such product");

        foreach (Product sellingProduct in Products.Where(sellingProduct => sellingProduct.Name == product.Name))
        {
            product.SetAmount(amount);
        }
    }

    public bool HasProduct(string nameOfProduct)
    {
        if (string.IsNullOrEmpty(nameOfProduct))
            throw new ShopException("empty");
        return Products.Any(product => product.Name == nameOfProduct);
    }

    public bool HasEnoughProduct(string nameOfProduct, int amount)
    {
        if (string.IsNullOrEmpty(nameOfProduct))
            throw new ShopException("empty");

        Product? product = Products.Find(p => p.Name == nameOfProduct);
        if (product is null)
            throw new ShopException("Not found");

        return product!.Amount >= amount;
    }

    public bool HasAllProducts(List<Product> orderList)
    {
        if (orderList is null)
        {
            throw new ShopException("There is no products in order list");
        }

        foreach (var curProduct in orderList)
        {
            Product? product = Products.Find(p => p.Name == curProduct.Name);
            if (product is null)
                throw new ShopException("Not found");
        }

        return true;
    }

    public int GetPrice(string nameOfProduct)
    {
        if (string.IsNullOrWhiteSpace(nameOfProduct))
            throw new ShopException("empty");

        Product? product = Products.Find(p => p.Name == nameOfProduct);

        if (product is null)
            throw new ShopException("Product is null");

        return product!.Price;
    }
}