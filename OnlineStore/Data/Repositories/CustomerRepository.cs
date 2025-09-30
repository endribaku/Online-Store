using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;
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

            DbParameter firstNameParam = cmd.CreateParameter(); // firstNameParam
            firstNameParam.ParameterName = "@FirstName";
            firstNameParam.Value = customer.FirstName;
            DbParameter lastNameParam = cmd.CreateParameter(); // lastNameParam
            lastNameParam.ParameterName = "@LastName";
            lastNameParam.Value = customer.LastName;
            cmd.Parameters.Add(firstNameParam);
            cmd.Parameters.Add(lastNameParam);

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
            DbParameter idParam = cmd.CreateParameter();
            idParam.ParameterName = "@customerId";
            idParam.Value = customerId;
            cmd.Parameters.Add(idParam);
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
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
            DbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customer WHERE CustomerId = @customerId";
            cmd.Transaction = this._unitOfWork.Transaction;
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
            _connection.Close();
        }

        return null;
    }
    

    public List<Customer> GetCustomers()
    {
        DbDataReader reader = null;
        try
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
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
            _connection.Close();
        }

        return null;
    }

    public void UpdateCustomer(Customer customer)
    {
        return;
    }
    
    
    
}