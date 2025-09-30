using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories;

public class CartRepository : ICartRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;
    
    public CartRepository(DbConnection dbConnection, IUnitOfWork unitOfWork)
    {
        this._connection = dbConnection;
        this._unitOfWork = unitOfWork;
    }

    public void CreateCart(Cart cart)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Cart (CustomerId) VALUES (@CustomerId)";
            cmd.Transaction = this._unitOfWork.Transaction;

            DbParameter customerIdParam = cmd.CreateParameter();
            customerIdParam.ParameterName = "@CustomerId";
            customerIdParam.Value = cart.CustomerId;
            cmd.Parameters.Add(customerIdParam);

            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to create cart");
            throw;
        }
    }

    public Cart GetCartById(int customerId)
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Cart WHERE CustomerId = @CustomerId";
            cmd.Transaction = this._unitOfWork.Transaction;

            DbParameter customerIdParam = cmd.CreateParameter();
            customerIdParam.ParameterName = "@CustomerId";
            customerIdParam.Value = customerId;
            cmd.Parameters.Add(customerIdParam);

            reader = cmd.ExecuteReader();
            Cart cart = new Cart();
            while (reader.Read())
            {
                cart.CartId = int.Parse(reader["CartId"].ToString()!);
                cart.CustomerId = int.Parse(reader["CustomerId"].ToString()!);
            }

            return cart;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to get cart");
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

    public void UpdateCart(Cart cart)
    {
        return;
    }

    public void DeleteCart(Cart cart)
    {
        return;
    }

    public IEnumerable<Cart> GetCarts()
    {
        return null;
    }
}