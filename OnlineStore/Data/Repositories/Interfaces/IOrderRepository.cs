
namespace OnlineStore.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    List<CustomerOrder> GetOrders();
    CustomerOrder GetOrderById(int orderId);
    void UpdateOrder(CustomerOrder order);
    void DeleteOrder(int orderId);
    
    void CreateOrder(CustomerOrder order);
}