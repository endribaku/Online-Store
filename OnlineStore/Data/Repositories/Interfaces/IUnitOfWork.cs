using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore.Data.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DbTransaction Transaction { get; }
    DbConnection Connection { get; }
    ICustomerRepository Customers { get; }
    ICartRepository Carts { get; }
    IProductRepository Products { get; }
    ICartItemRepository CartItems { get; }
    IOrderLineRepository OrderLines { get; }
    
    IOrderRepository Orders { get; }
    
    void Commit();
    void Rollback();
    
}