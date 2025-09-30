using System.Data.Common;
using OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;
namespace OnlineStore.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private DbConnection Connection { get; set; }
    

    public CustomerRepository(DbConnection dbConnection)
    {
        this.Connection = Connection;
    }

    public void AddCustomer(Customer customer)
    {
        DbCommand cmd = Connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Customer (FirstName, LastName) VALUES (@FirstName, @LastName)";
            
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

    public void DeleteCustomer(int customerId)
    {
        DbCommand cmd = Connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Customer WHERE CustomerId = @customerId";
        DbParameter idParam = cmd.CreateParameter();
        idParam.ParameterName = "@customerId";
        idParam.Value = customerId;
        cmd.Parameters.Add(idParam);
        cmd.ExecuteNonQuery();
    }

    public Customer GetCustomerById(int customerId)
    {
        DbDataReader reader = null;
        try
        {
            Connection.Open();
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customer WHERE CustomerId = @customerId";
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
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            Connection.Close();
        }

        return null;
    }
    

    public List<Customer> GetCustomers()
    {
        DbDataReader reader = null;
        try
        {
            Connection.Open();
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Customer;";
            reader = cmd.ExecuteReader();
            List<Customer> customers = new List<Customer>();
            while (reader.Read())
            {
                Customer customer = new Customer();
                customer.FirstName = reader["FirstName"].ToString()!;
                customer.LastName = reader["LastName"].ToString()!;

                customers.Add(customer);
            }

            return customers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            Connection.Close();
        }

        return null;
    }

    public void UpdateCustomer(Customer customer)
    {
        return;
    }
    
    
    
}