using Shops.Entities;
using Shops.Services;
using Shops.Tools;
using Xunit;

namespace Shops.Test;

public class ShopTests
{
    private ShopOperation _shopOperation = new ShopOperation();

    [Fact]
    public void CreateShopAddProductsToShop_CustomerCanBuy()
    {
        Shop shop = _shopOperation.AddShop("Tesco", "Bloomenland ave, 3");
        Customer customer = new Customer("Danny", 120);
        string product1 = _shopOperation.RegisterProduct("banana");
        string product2 = _shopOperation.RegisterProduct("Snickers");

        _shopOperation.AddProductToShop(shop, product1, 60, 10);
        _shopOperation.AddProductToShop(shop, product2, 50, 1);
        _shopOperation.Purchase(shop, customer, product1, 2);

        Assert.True(shop.HasProduct("banana"));
        Assert.True(shop.HasProduct("Snickers"));
        Assert.False(shop.HasEnoughProduct("banana", 10));
        Assert.True(shop.HasEnoughProduct("banana", 8));
        Assert.True(customer.Balance == 0);
    }

    [Fact]
    public void SetAndChangePricesInShop_PricesChanged()
    {
        Shop shop = _shopOperation.AddShop("Whole Foods Market", "10 Piccadilly Circus");
        string product1 = _shopOperation.RegisterProduct("Lays Crab");
        string product2 = _shopOperation.RegisterProduct("Salsa");

        _shopOperation.AddProductToShop(shop, product1, 100, 30);
        _shopOperation.AddProductToShop(shop, product2, 50, 20);
        int firstPriceOnProduct1 = shop.GetPrice(product1);
        int firstPriceOnProduct2 = shop.GetPrice(product2);
        _shopOperation.ChangePriceOfProduct(shop, product1, 120);
        _shopOperation.ChangePriceOfProduct(shop, product2, 70);

        Assert.True(firstPriceOnProduct1 == 100);
        Assert.True(firstPriceOnProduct2 == 50);
        Assert.True(shop.GetPrice(product1) == 120);
        Assert.True(shop.GetPrice(product2) == 70);
    }

    [Fact]
    public void FindShopWithBestPrices_ShopIsCorrect()
    {
        Shop shop1 = _shopOperation.AddShop("Whole Foods Market", "10 Piccadilly Circus");
        Shop shop2 = _shopOperation.AddShop("Tesco", "Bloomenland ave, 3");
        Shop shop3 = _shopOperation.AddShop("One pound", "Westminster ave, 100");
        string product1 = _shopOperation.RegisterProduct("Lays Crab");
        string product2 = _shopOperation.RegisterProduct("Coca Cola");
        var fisrtProduct = new Product("Lays Crab", 30, 5);
        var secondProduct = new Product("Coca Cola", 60, 3);
        var orderList = new List<Product>
        {
            fisrtProduct,
            secondProduct,
        };

        _shopOperation.AddProductToShop(shop1, product1, 50, 10);
        _shopOperation.AddProductToShop(shop1, product2, 80, 10);
        _shopOperation.AddProductToShop(shop2, product1, 45, 8);
        _shopOperation.AddProductToShop(shop2, product2, 70, 7);
        _shopOperation.AddProductToShop(shop3, product1, 30, 5);
        _shopOperation.AddProductToShop(shop3, product2, 60, 3);

        Assert.True(_shopOperation.CheapestShop(orderList) == shop3);
    }

    [Fact]
    public void BuyBunchOfProducts_BalanceAndAmountHaveChanged()
    {
        Customer customer = new Customer("Max", 1000);
        Shop shop1 = _shopOperation.AddShop("Whole Foods Market", "10 Piccadilly Circus");
        string product1 = _shopOperation.RegisterProduct("Kinder Bueno");
        string product2 = _shopOperation.RegisterProduct("Corona Extra");
        string product3 = _shopOperation.RegisterProduct("Nachos");

        _shopOperation.AddProductToShop(shop1, product1, 85, 5);
        _shopOperation.AddProductToShop(shop1, product2, 120, 9);
        _shopOperation.AddProductToShop(shop1, product3, 60, 13);
        _shopOperation.Purchase(shop1, customer, product1, 5);
        int customerBalance1 = customer.Balance;
        bool hasProductsAfterFirst = shop1.HasEnoughProduct(product1, 1);
        _shopOperation.Purchase(shop1, customer, product2, 4);
        int customerBalance2 = customer.Balance;
        bool hasProductsAfterSecond = shop1.HasEnoughProduct(product2, 6);

        Assert.False(hasProductsAfterFirst);
        Assert.True(customerBalance1 == 575);
        Assert.False(hasProductsAfterSecond);
        Assert.True(customerBalance2 == 95);
    }
}