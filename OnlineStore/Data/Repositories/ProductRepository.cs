using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;

namespace OnlineStore.Data.Repositories;

public class ProductRepository: IProductRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;


    public ProductRepository(DbConnection connection, IUnitOfWork unitOfWork)
    {
        _connection = connection;
        _unitOfWork = unitOfWork;
    }
    public List<Product> GetProducts()
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT *  FROM Product";
            cmd.Transaction = this._unitOfWork.Transaction;
            reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product();
                product.ProductId = int.Parse(reader["ProductId"].ToString()!);
                product.Name = reader["Name"].ToString()!;
                product.Price = decimal.Parse(reader["Price"].ToString()!);

                products.Add(product);
            }

            return products;
        }
        catch (Exception)
        {
            Console.WriteLine("Couldn't get products");
            throw;
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
    }

    public Product GetProductById(int id)
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT *  FROM Product WHERE ProductId = @ProductId";
            cmd.Transaction = this._unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@ProductId", id);

            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Product product = new Product();
                product.ProductId = int.Parse(reader["productId"].ToString()!);
                product.Name = reader["Name"].ToString()!;
                product.Price = decimal.Parse(reader["Price"].ToString()!);
                

                return product;
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Couldn't get product");
            throw;
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }

        return null;
    }

    public void CreateProduct(Product product)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Product (Name, Price) VALUES (@Name, @Price)";
            cmd.Transaction = this._unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@Name", product.Name);
            ParameterHelper.AddParameter(cmd, "@Price", product.Price);

            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Console.WriteLine("Couldn't add product");
            throw;
        }
    }

    public void DeleteProduct(int productId)
    {
        return;
    }

    public void GetProduct(Product product)
    {
        return;
    }

    public void UpdateProduct(Product product)
    {
        return;
    }
}
