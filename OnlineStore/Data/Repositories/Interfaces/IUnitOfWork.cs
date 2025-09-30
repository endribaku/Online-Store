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
    
    void Commit();
    void Rollback();
    
}