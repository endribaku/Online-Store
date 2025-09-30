
using OnlineStoreLibrary;
namespace OnlineStore.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    IEnumerable<Order> GetOrders();
    Order GetOrderById(int orderId);
    void GetOrder(Order order);
    void UpdateOrder(Order order);
    void DeleteOrder(int orderId);
}