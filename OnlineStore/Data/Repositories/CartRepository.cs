using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;
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
            
            ParameterHelper.AddParameter(cmd, "@CustomerId", cart.CustomerId);

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
            Cart cart = null;
            while (reader.Read())
            {
                cart = new Cart(int.Parse(reader["CartId"].ToString()!));
                cart.CustomerId = int.Parse(reader["CustomerId"].ToString()!);
            }

            if (cart == null)
            {
                throw new Exception();
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