namespace OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;
public interface IProductRepository
{
    IEnumerable<Product> GetProducts();
    Product GetProductById(int productId);
    void GetProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int productId);
}