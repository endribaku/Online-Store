using System.Collections.ObjectModel;

namespace OnlineStore;
using OnlineStoreClassLibrary;
public class OnlineStoreSystem
{
    private List<Product> _products;
    private List<Order> _orders;
    private List<Customer> _customers;
    private Customer? _activeCustomer = null;

    public OnlineStoreSystem()
    {
        _products = new List<Product>();
        _orders = new List<Order>();
        _customers = new List<Customer>();
        _activeCustomer = null;
    }

    public OnlineStoreSystem(List<Product> products)
    {
        _products = products;
        _orders = new List<Order>();
        _customers = new List<Customer>();
        _activeCustomer = null;
    }

    public bool AddProduct(string name, decimal price)
    {
        if (_products.Find(p => p.Name == name) == null)
        {
            _products.Add(new Product(name, price));
            return true;
        }

        return false;
    }

    public bool CheckProductByName(string name)
    {
        return _products.Find(p => p.Name == name) != null;
    }

    public void AddCustomer(string name)
    {
        _customers.Add(new Customer(name));
    }

    public Order PlaceOrder(Customer customer)
    {
        Order order = customer.Checkout();
        _orders.Add(order);
        return order;
    }

    public void SelectCustomer(int id)
    {
        _activeCustomer = _customers[id];
    }

    public void DisplayProducts()
    {
        foreach (Product product in _products)
        {
            Console.WriteLine(product.ToString());
        }
    }

    public void DisplayCustomers()
    {
        if (_customers.Count == 0)
        {
            Console.WriteLine("No Customers Found");
            return;
        }

        foreach (Customer customer in _customers)
        {
            if (customer == _activeCustomer)
            {
                Console.WriteLine(customer.ToString() + " (is active)");
            }
            else
            {
                Console.WriteLine(customer.ToString());
            }
            
        }
    }

    public bool CheckCustomerByName(string name)
    {
        return _customers.Find(c => c.Name == name) != null;
    }

    public bool SelectActiveCustomer(int id)
    {
        if (id < 0 || id >= _customers.Count) return false;
        
        _activeCustomer = _customers[id];
        return true;
    }

    public List<ProductDto> GetProducts()
    {
        return _products.Select(p => new ProductDto(p.ProductId, p.Name, p.Price)).ToList();
    }

    public List<CustomerDto> GetCustomers()
    {
        return _customers.Select(c => new CustomerDto(c.CustomerId, c.Name)).ToList();
    }

    public List<OrderDto> GetOrders()
    {
        return _orders.Select(o => new OrderDto(o.OrderId, o.OrderTotal, new CustomerDto(o.Customer!.CustomerId,o.Customer.Name), o.OrderDate)).ToList();
    }

    public bool HasActiveCustomer()
    {
        if (_activeCustomer == null) return false;
        return true;
    }
    
    public List<CartItemDto> GetActiveCustomerCart()
    {
        if (!HasActiveCustomer())
        {
            return null;
        }

        return _activeCustomer!.GetShoppingCart();
    }

    public bool CheckProductById(int id)
    {
        if (id < 0 || id >= _products.Count) return false;

        return true;
    }

    public bool AddToCart(int id, int quantity)
    {
        if (!CheckProductById(id)) return false;
        
        _activeCustomer.AddProduct(new CartItem(_products[id], quantity));
        return true;
    }

    public OrderDto Checkout()
    {
        if (_activeCustomer == null) return null;
        
        Order order = _activeCustomer.Checkout();
        if(order == null) return null;
        
        _orders.Add(order);
        return new OrderDto(order.OrderId, order.OrderTotal, new CustomerDto(order.Customer!.CustomerId, order.Customer.Name), order.OrderDate);
        
    }
    
    
    
    
    
    
}