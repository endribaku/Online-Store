using System.Data.Common;
using System.Reflection.Metadata;
using MySqlConnector;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;
using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories;

public class CartItemRepository: ICartItemRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;

    public CartItemRepository(DbConnection connection, IUnitOfWork unitOfWork)
    {
        this._connection = connection;
        this._unitOfWork = unitOfWork;
    }

    public List<CartItem> GetCartItems(int cartId)
    {
        DbDataReader reader = null;
        try
        {

            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT ci.CartItemId, ci.Quantity, ci.ProductId, ci.CartId, p.Name, p.Price " +
                              "FROM CartItem AS ci, Product AS p WHERE ci.ProductId = p.ProductId AND ci.CartId = @cartId";
            cmd.Transaction = _unitOfWork.Transaction;

            
            ParameterHelper.AddParameter(cmd, "@cartId", cartId);

            List<CartItem> cartItems = new List<CartItem>();
            
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CartItem cartItem = new CartItem(int.Parse(reader["CartItemId"].ToString()!));
                cartItem.CartId = int.Parse(reader["CartId"].ToString()!);
                cartItem.Quantity = int.Parse(reader["Quantity"].ToString()!);
                cartItem.ProductId = int.Parse(reader["ProductId"].ToString()!);
                Product product = new Product(cartItem.ProductId);
                product.Name = reader["Name"].ToString()!;
                product.Price = decimal.Parse(reader["Price"].ToString()!);
                cartItem.Product = product;
                cartItems.Add(cartItem);
            }
            
            return cartItems;
        }
        catch (Exception e )
        {
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Failed to get cart items");
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

    public void CreateCartItem(CartItem cartItem)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO CartItem(Quantity, ProductId, CartId) VALUES (@Quantity, @ProductId, @CartId)";
            cmd.Transaction = _unitOfWork.Transaction;

            DbParameter quantityParameter = cmd.CreateParameter();
            quantityParameter.ParameterName = "@Quantity";
            quantityParameter.Value = cartItem.Quantity;

            DbParameter productIdParameter = cmd.CreateParameter();
            productIdParameter.ParameterName = "@ProductId";
            productIdParameter.Value = cartItem.ProductId;

            DbParameter cartIdParameter = cmd.CreateParameter();
            cartIdParameter.ParameterName = "@CartId";
            cartIdParameter.Value = cartItem.CartId;

            cmd.Parameters.Add(quantityParameter);
            cmd.Parameters.Add(productIdParameter);
            cmd.Parameters.Add(cartIdParameter);

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to create cart item");
            throw;
        }
    }

    public void UpdateCartItem(CartItem cartItem)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText =
                "UPDATE CartItem SET Quantity = @Quantity, ProductId = @ProductId, CartId = @CartId WHERE CartItemId = @CartId";
            cmd.Transaction = _unitOfWork.Transaction;

            
            ParameterHelper.AddParameter(cmd, "@Quantity", cartItem.Quantity);
            ParameterHelper.AddParameter(cmd, "@ProductId", cartItem.ProductId);
            ParameterHelper.AddParameter(cmd, "@CartId", cartItem.CartId);
            

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to update cart item");
            throw;
        }
    }

    public void DeleteCartItem(CartItem cartItem)
    {
        return;
    }

    public CartItem GetCartItemById(int cartId, int productId)
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM CartItem WHERE CartId = @CartId AND ProductId = @ProductId";
            cmd.Transaction = _unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@CartId", cartId);
            ParameterHelper.AddParameter(cmd, "@ProductId", productId);
           

            reader = cmd.ExecuteReader();
            CartItem cartItem = null;
            if (reader.Read())
            {
                cartItem = new CartItem(int.Parse(reader["CartItemId"].ToString()!));
                cartItem.Quantity = int.Parse(reader["Quantity"].ToString()!);
                cartItem.ProductId = int.Parse(reader["ProductId"].ToString()!);
                cartItem.CartId = int.Parse(reader["CartId"].ToString()!);
            }

            return cartItem;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to get cart item");
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
    
}