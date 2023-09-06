using Shops.Entities;

namespace Shops.Services;

public interface IShopOperation
{
    Shop AddShop(string name, string address);

    Shop CheapestShop(List<Product> orderList);
    string RegisterProduct(string productName);
    void AddProductToShop(Shop shop, string nameOfProduct, int price, int amount);
    void ChangePriceOfProduct(Shop shop, string nameOfProduct, int newPrice);
    void Purchase(Shop shop, Customer customer, string nameOfProduct, int amount);
}