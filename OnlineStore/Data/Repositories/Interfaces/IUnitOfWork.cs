using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore.Data.Repositories.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    
    void Commit();
    void Rollback();
}