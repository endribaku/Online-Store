namespace OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;

public interface ICustomerRepository
{
    List<Customer> GetCustomers();
    Customer GetCustomerById(int customerId);
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
}