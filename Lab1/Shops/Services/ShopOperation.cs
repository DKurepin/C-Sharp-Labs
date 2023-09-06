using Shops.Entities;
using Shops.Tools;

namespace Shops.Services;

public class ShopOperation : IShopOperation
{
    private List<Shop> _shops = new List<Shop>();
    private List<string> _products = new List<string>();

    public Shop AddShop(string name, string address)
    {
        var shop = new Shop(name, address, _shops.Count);
        _shops.Add(shop);
        return shop;
    }

    public Shop CheapestShop(List<Product> orderList)
    {
        if (orderList == null)
        {
            throw new ShopException("There is an empty list");
        }

        if (_shops.Count == 0)
            throw new ShopException("Shop does not exist");

        Shop? bestShop = null;
        int maxCost = int.MaxValue;

        foreach (Shop minShop in _shops)
        {
            if (!minShop.HasAllProducts(orderList))
                continue;
            int costOfProducts = orderList.Sum(product => minShop.GetPrice(product.Name));

            if (costOfProducts >= maxCost)
                continue;
            maxCost = costOfProducts;
            bestShop = minShop;
        }

        if (bestShop == null)
            throw new ShopException("There is no suitable shop");

        return bestShop!;
    }

    public string RegisterProduct(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ShopException("Name of product is incorrect. It should be letters");
        }

        _products.Add(productName);
        return productName;
    }

    public void AddProductToShop(Shop shop, string nameOfProduct, int price, int amount)
    {
        var product = new Product(nameOfProduct, price, amount);
        shop.Products.Add(product);
    }

    public void ChangePriceOfProduct(Shop shop, string nameOfProduct, int newPrice)
    {
        foreach (var product in shop.Products.Where(product => product.Name.Equals(nameOfProduct)))
        {
            product.SetPrice(newPrice);
        }
    }

    public void Purchase(Shop shop, Customer customer, string nameOfProduct, int amount)
    {
        int overallCost = amount * shop.GetPrice(nameOfProduct);
        if (customer.HasEnoughMoney(overallCost))
        {
            shop.BuyProduct(nameOfProduct, amount);
            customer.ChangeBalance(-(amount * shop.GetPrice(nameOfProduct)));
        }
    }
}