using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private DbConnection _connection;
    private DbTransaction _transaction;
    public ICustomerRepository Customers { get; }
    
    public UnitOfWork(DbConnection connection)
    {
        _connection = connection;
        _transaction = _connection.BeginTransaction();
        Customers = new CustomerRepository(_connection);
    }
    
    public void Commit()
    {
        return;
    }

    public void Rollback()
    {
        return;
    }
}