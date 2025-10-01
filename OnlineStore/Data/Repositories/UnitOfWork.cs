using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public DbConnection Connection {get; set;}
    public DbTransaction Transaction {get; set;}
    public ICustomerRepository Customers { get; }
    public ICartRepository Carts { get; }
    public IProductRepository Products { get; }
    
    public ICartItemRepository CartItems { get; }
    
    
    
    public UnitOfWork(DbProviderFactory factory, string connectionString)
    {
        Connection = factory.CreateConnection();
        Connection.ConnectionString = connectionString;
        Connection.Open();
        
        Transaction = Connection.BeginTransaction();
        Customers = new CustomerRepository(Connection, this);
        Carts = new CartRepository(Connection, this);
        Products = new ProductRepository(Connection, this);
        CartItems = new CartItemRepository(Connection, this);
    }
    
    public void Commit()
    {
        Transaction.Commit();
    }

    public void Rollback()
    {
        Transaction.Rollback();
    }

    public void Dispose()
    {
        Connection.Dispose();
        Transaction.Dispose();
    }
}