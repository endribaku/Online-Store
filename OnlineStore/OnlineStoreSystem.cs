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
    private int _activeCustomerId = -1; // id in DB Schema starts from 1 and auto increments by 1

    public OnlineStoreSystem(DbProviderFactory factory, string connectionString)
    {
       this._providerFactory = factory;
       this._connectionString = connectionString;
    }
    
    public bool AddProduct(string name, decimal price)
    {
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                Product newProduct = new Product();
                newProduct.Name = name;
                newProduct.Price = price;
                uow.Products.CreateProduct(newProduct);
                uow.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                uow.Rollback();
                return false;
            }
        }
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
                
                
                
                DbCommand lastInsertedCommand = uow.Connection.CreateCommand();
                lastInsertedCommand.CommandText = "SELECT LAST_INSERT_ID() AS id_value"; // this command will likely be a util function in the future
                lastInsertedCommand.Transaction = uow.Transaction;
                DbDataReader reader = lastInsertedCommand.ExecuteReader();
                int customerId = -1;
                while (reader.Read())
                {
                   customerId = int.Parse(reader["id_value"].ToString()!);
                }
                
                reader.Close();
                
                //adding cart needed before committing the transaction (DONE)
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

    public bool SelectActiveCustomer(int id)
    {
        List < Customer > customers = GetCustomers();
        if (customers.Find((c) => c.CustomerId == id) == null) return false;
        
        _activeCustomerId = id;
        return true;

    }

    public List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                products = uow.Products.GetProducts();
                uow.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }

        return products;
    }

    public List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory,  this._connectionString))
        {
            try
            {
                customers = uow.Customers.GetCustomers();
                uow.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }

        return customers;

    }

    public List<CustomerOrder> GetOrders()
    {
        List<CustomerOrder> orders = new List<CustomerOrder>();
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                orders = uow.Orders.GetOrders();
                uow.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }

        return orders;
    }

    public bool HasActiveCustomer()
    {
        if (_activeCustomerId < 1)
        {
            return false;
        }

        return true;
    }
    
    public Cart GetActiveCustomerCart()
    {
        if (!HasActiveCustomer())
        {
            return null;
        }
        Cart cart = new Cart();
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                cart = uow.Carts.GetCartById(_activeCustomerId);
                List<CartItem> cartItems = uow.CartItems.GetCartItems(cart.CartId);
                cart.Items = cartItems;
                uow.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }
        
        return cart;
    }

    public bool CheckProductById(int id)
    {
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                Product product = uow.Products.GetProductById(id);
                uow.Commit();
                if (product != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
                return false;
            }
        }
    }

    public bool AddToCart(int productId, int quantity)
    {
        if (!HasActiveCustomer())
        {
            return false;
        }
        CartItem cartItem = new CartItem();
        int cartItemId = -1;
        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                cartItem = new CartItem();
                cartItem.ProductId = productId;
                cartItem.Quantity = quantity;
                cartItem.CartId = GetActiveCustomerCart().CartId;
                
                uow.CartItems.CreateCartItem(cartItem);
                uow.Commit();
                return true;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry"))
                {
                    Console.WriteLine("Entry already exists. Updating quantity...");
                    CartItem item = GetActiveCustomerCart().Items.Find(existingItem => existingItem.ProductId == productId)!; 
                    item.Quantity = quantity;
                    uow.CartItems.UpdateCartItem(item);
                    uow.Commit();
                    return true;
                }
                else
                {
                    uow.Rollback();
                    return false;
                }
                
            }
        }
    }

    public CustomerOrder Checkout()
    {
        if (!HasActiveCustomer())
        {
            return null;
        }

        CustomerOrder order = null;

        using (IUnitOfWork uow = new UnitOfWork(this._providerFactory, this._connectionString))
        {
            try
            {
                Customer customer = uow.Customers.GetCustomerById(_activeCustomerId);
                List<CartItem> items = GetActiveCustomerCart().Items;
                if (items.Count == 0)
                {
                    return null;
                }
                Decimal totalPrice = items.Sum(item => item.Product.Price * item.Quantity);

                order = new CustomerOrder(DateTime.Now);
                order.Total = totalPrice;
                order.CustomerId = _activeCustomerId;
                order.CustomerName = customer.FirstName + " " + customer.LastName;

                uow.Orders.CreateOrder(order);
                uow.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                uow.Rollback();
            }
        }

        return order;

    }
    
    
    
    
    
    
}