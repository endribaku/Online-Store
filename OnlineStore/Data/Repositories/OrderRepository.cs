using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;

namespace OnlineStore.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;

    public OrderRepository(DbConnection connection, IUnitOfWork unitOfWork)
    {
        _connection = connection;
        _unitOfWork = unitOfWork;
    }

    public void CreateOrder(CustomerOrder order)
    {
        try
        {
            DbCommand command = _connection.CreateCommand();
            command.CommandText =
                "INSERT INTO CustomerOrder(Total, CustomerName, CustomerId) VALUES (@Total, @CustomerName, @CustomerId)";
            command.Transaction = _unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(command, "@Total", order.Total);
            ParameterHelper.AddParameter(command, "@CustomerName", order.CustomerName);
            ParameterHelper.AddParameter(command, "@CustomerId", order.CustomerId);

            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Console.WriteLine("Could not create order");
            throw;
        }
    }

    public void DeleteOrder(int orderId)
    {
        throw new NotImplementedException();
    }

    public CustomerOrder GetOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public List<CustomerOrder> GetOrders()
    {
        DbDataReader reader = null;
        try
        {
            DbCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM CustomerOrder ORDER BY Date DESC";
            command.Transaction = _unitOfWork.Transaction;

            List<CustomerOrder> orders = new List<CustomerOrder>();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                CustomerOrder order = new CustomerOrder();
                order.OrderId = int.Parse(reader["OrderId"].ToString())!;
                order.Date = reader.GetDateTime(reader.GetOrdinal("Date"));
                order.CustomerId = int.Parse(reader["CustomerId"].ToString()!);
                order.CustomerName = reader["CustomerName"].ToString()!;
                order.Total = decimal.Parse(reader["Total"].ToString()!);
                orders.Add(order);
            }

            return orders;
        }
        catch (Exception)
        {
            Console.WriteLine("Could not get orders");
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

    public void UpdateOrder(CustomerOrder order)
    {
        throw new NotImplementedException();
    }
}