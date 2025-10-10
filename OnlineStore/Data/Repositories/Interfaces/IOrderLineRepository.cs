namespace OnlineStore.Data.Repositories.Interfaces;

public interface IOrderLineRepository
{
    List<OrderLine> GetOrderLines(int orderId);
    OrderLine GetOrderLine(int orderLineId);
    void CreateOrderLine(OrderLine orderLine);
    void UpdateOrderLine(OrderLine orderLine);
    void DeleteOrderLine(OrderLine orderLine);
}