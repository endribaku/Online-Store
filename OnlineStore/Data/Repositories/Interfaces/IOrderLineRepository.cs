using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories.Interfaces;

public interface IOrderLineRepository
{
    IEnumerable<OrderLine> GetOrderLines();
    OrderLine GetOrderLine(int orderLineId);
    void CreateOrderLine(OrderLine orderLine);
    void UpdateOrderLine(OrderLine orderLine);
    void DeleteOrderLine(OrderLine orderLine);
}