using System.Data.Common;
using System.Reflection.Metadata;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;
namespace OnlineStore.Data.Repositories;

public class OrderLineRepository: IOrderLineRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;

    public OrderLineRepository(DbConnection connection, IUnitOfWork unitOfWork)
    {
        _connection = connection;
        _unitOfWork = unitOfWork;
    }

    public void CreateOrderLine(OrderLine orderLine)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText =
                "INSERT INTO OrderLine(Quantity, UnitPrice, LineTotal, ProductName, ProductId, OrderId) VALUES " +
                "(@Quantity, @UnitPrice, @LineTotal, @ProductName, @ProductId, @OrderId)";
            cmd.Transaction = _unitOfWork.Transaction;

            ParameterHelper.AddParameter(cmd, "Quantity", orderLine.Quantity);
            ParameterHelper.AddParameter(cmd, "UnitPrice", orderLine.UnitPrice);
            ParameterHelper.AddParameter(cmd, "LineTotal", orderLine.LineTotal);
            ParameterHelper.AddParameter(cmd, "ProductName", orderLine.ProductName);
            ParameterHelper.AddParameter(cmd, "ProductId", orderLine.ProductId!);
            ParameterHelper.AddParameter(cmd, "OrderId", orderLine.OrderId);
            
            cmd.ExecuteNonQuery();

        }
        catch (Exception)
        {
            Console.WriteLine("Error while creating order line: " + orderLine.ProductId);
            throw;
        }
    }

    public void DeleteOrderLine(OrderLine orderLine)
    {
        throw new NotImplementedException();
    }

    public OrderLine GetOrderLine(int orderLineId)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderLine(OrderLine orderLine)
    {
        throw new NotImplementedException();
    }

    public List<OrderLine> GetOrderLines(int orderId)
    {
        DbDataReader reader = null;
        try
        {
            DbCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM OrderLine WHERE OrderId = @OrderId";
            command.Transaction = _unitOfWork.Transaction;

            ParameterHelper.AddParameter(command, "@OrderId", orderId);

            reader = command.ExecuteReader();
            List<OrderLine> lines = new List<OrderLine>();
            while (reader.Read())
            {
                OrderLine orderLine = new OrderLine();
                orderLine.OrderLineId = int.Parse(reader["OrderLineId"].ToString()!);
                orderLine.Quantity = int.Parse(reader["Quantity"].ToString()!);
                orderLine.UnitPrice = decimal.Parse(reader["UnitPrice"].ToString()!);
                orderLine.LineTotal = decimal.Parse(reader["LineTotal"].ToString()!);
                orderLine.ProductName = reader["ProductName"].ToString()!;
                orderLine.ProductId = int.Parse(reader["ProductId"].ToString()!);
                orderLine.OrderId = orderId;
                lines.Add(orderLine);
            }

            return lines;
        }
        catch (Exception)
        {
            Console.WriteLine("Error while reading order line: " + orderId);
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