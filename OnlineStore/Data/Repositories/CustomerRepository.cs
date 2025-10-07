using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStore.Utilities;

namespace OnlineStore.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private DbConnection _connection;
    private IUnitOfWork _unitOfWork;
    

    public CustomerRepository(DbConnection dbConnection, IUnitOfWork unitOfWork)
    {
        this._connection = dbConnection;
        this._unitOfWork = unitOfWork;
    }

    public void AddCustomer(Customer customer)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Customer (FirstName, LastName) VALUES (@FirstName, @LastName)";
            cmd.Transaction = this._unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@FirstName", customer.FirstName);
            ParameterHelper.AddParameter(cmd, "@LastName", customer.LastName);
            

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        

    }

    public void DeleteCustomer(int customerId)
    {
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Customer WHERE CustomerId = @customerId";
            cmd.Transaction = this._unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@customerId", customerId);
            
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Console.WriteLine("Error deleting customer");
            throw;
        }
        
    }

    public Customer GetCustomerById(int customerId)
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customer WHERE CustomerId = @customerId";
            cmd.Transaction = this._unitOfWork.Transaction;
            
            ParameterHelper.AddParameter(cmd, "@customerId", customerId);
            
            
            reader = cmd.ExecuteReader();
            Customer customer = new Customer();
            while (reader.Read())
            {
                customer.FirstName = reader["FirstName"].ToString()!;
                customer.LastName = reader["LastName"].ToString()!;
            }

            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error getting customer");
            throw;
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }

        return null;
    }
    

    public List<Customer> GetCustomers()
    {
        DbDataReader reader = null;
        try
        {
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customer";
            cmd.Transaction = this._unitOfWork.Transaction;
            reader = cmd.ExecuteReader();
            List<Customer> customers = new List<Customer>();
            while (reader.Read())
            {
                Customer customer = new Customer();
                Console.WriteLine(reader["CustomerId"].ToString());
                customer.CustomerId = int.Parse(reader["CustomerId"].ToString()!);
                customer.FirstName = reader["FirstName"].ToString()!;
                customer.LastName = reader["LastName"].ToString()!;

                customers.Add(customer);
            }
            
            return customers;
        }
        catch (Exception)
        {
            Console.WriteLine("Error getting customers");
            throw;
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }

        return null;
    }

    public void UpdateCustomer(Customer customer)
    {
        return;
    }
    
    
    
}