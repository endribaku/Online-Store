using System.Collections.ObjectModel;
using System.Data.Common;
using OnlineStore.Data.Repositories;
using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore;
using OnlineStoreClassLibrary;
public class OnlineStoreSystem
{
    private DbProviderFactory _providerFactory;
    private string _connectionString;

    public OnlineStoreSystem(DbProviderFactory factory, string connectionString)
    {
       this._providerFactory = factory;
       this._connectionString = connectionString;
    }
    
    public bool AddProduct(string name, decimal price)
    {
        return false;
    }

    public bool CheckProductByName(string name)
    {
        return false;
    }

    public void AddCustomer(string firstName, string lastName)
    {
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory,  this._connectionString))
        {
            try
            {
                Customer newCustomer = new Customer();
                newCustomer.FirstName = firstName;
                newCustomer.LastName = lastName;
                uow.Customers.AddCustomer(newCustomer);
                
                DbCommand LastInsertedCommand = uow.Connection.CreateCommand();
                LastInsertedCommand.CommandText = "SELECT LAST_INSERT_ID() AS id_value;";
                LastInsertedCommand.Transaction = uow.Transaction;
                DbDataReader reader = LastInsertedCommand.ExecuteReader();
                int customerId = -1;
                while (reader.Read())
                {
                   customerId = int.Parse(reader["id_value"].ToString()!);
                }
                

                reader.Close();
                //commit cart needed before committing the transaction (TO BE DONE)
                Cart newCart = new Cart();
                newCart.CustomerId = customerId;
                uow.Carts.CreateCart(newCart);
                
                uow.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                uow.Rollback();
            }
        }
    }

    public Order PlaceOrder(Customer customer)
    {
        return null;
    }

    public void SelectCustomer(int id)
    {
        return;
    }

    public void DisplayProducts()
    {
        return;
    }

    

    public bool CheckCustomerByName(string name)
    {
        return false;
    }

    public bool SelectActiveCustomer(int id)
    {
        return false;
    }

    public List<ProductDto> GetProducts()
    {
        return null;
    }

    public List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory,  this._connectionString))
        {
            try
            {
                customers = uow.Customers.GetCustomers();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }

        return customers;

    }

    public List<OrderDto> GetOrders()
    {
        return null;
    }

    public bool HasActiveCustomer()
    {
        return false;
    }
    
    public List<CartItemDto> GetActiveCustomerCart()
    {
        return null;
    }

    public bool CheckProductById(int id)
    {
        return false;
    }

    public bool AddToCart(int id, int quantity)
    {
        return false;
    }

    public OrderDto Checkout()
    {
        return null;

    }
    
    
    
    
    
    
}