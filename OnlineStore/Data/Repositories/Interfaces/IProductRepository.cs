namespace OnlineStore.Data.Repositories.Interfaces;

public interface IProductRepository
{
    List<Product> GetProducts();
    Product GetProductById(int productId);
    void GetProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int productId);
    
    void CreateProduct(Product product);
}