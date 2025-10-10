using System.Collections.ObjectModel;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data.Repositories;
using OnlineStore.Data.Repositories.Interfaces;

namespace OnlineStore;

public class OnlineStoreSystem
{
    private int _activeCustomerId = -1; // id in DB Schema starts from 1 and auto increments by 1

    public OnlineStoreSystem()
    {
    }
    
    public bool AddProduct(string name, decimal price)
    {
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Product product = new Product() {Name = name, Price = price};
                context.Products.Add(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to add product");
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
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Cart cart = new Cart()
                {
                    Customer = new Customer() { FirstName = firstName, LastName = lastName }
                };
                    
                context.Carts.Add(cart);
                context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to add customer");
                throw;
            }
        }
    }

    public bool CheckCustomerById(int customerId)
    {
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Customer customer = context.Customers.SingleOrDefault(c => c.CustomerId == customerId);
                if (customer != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to find customer");
                throw;
            }
            
        }
    }

    public void DeleteCustomer(int customerId)
    {
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Customer customer = context.Customers.SingleOrDefault(c => c.CustomerId == customerId);
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to delete customer");
                throw;
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
        using (var context = new OnlineStoreContext())
        {
            products = context.Products.ToList();
        }

        return products;
    }

    public List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        using (var context = new OnlineStoreContext())
        {
            customers = context.Customers.AsNoTracking().ToList();
        }

        return customers;

    }

    public List<CustomerOrder> GetOrders()
    {
        List<CustomerOrder> orders = new List<CustomerOrder>();
        using (var context = new OnlineStoreContext())
        {
            orders = context.CustomerOrders.AsNoTracking().ToList();
        }

        return orders;
    }

    public bool HasActiveCustomer()
    {
        List<Customer> customers = GetCustomers();

        if (customers.Find(c => c.CustomerId == _activeCustomerId) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsActiveCustomer(int customerId)
    {
        if(_activeCustomerId == customerId)  return true;
        return false;
    }
    
    public Cart GetActiveCustomerCart()
    {
        if (!HasActiveCustomer())
        {
            return null;
        }
        Cart cart = new Cart();
        using (var context = new OnlineStoreContext())
        {
            try
            {
                cart = context.Carts.AsNoTracking().Include(c=> c.CartItems).SingleOrDefault(c => c.CustomerId == _activeCustomerId)!;
                foreach (var item in cart.CartItems)
                {
                    item.Product = context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == item.ProductId)!;
                }

                cart.Customer = context.Customers.FirstOrDefault(c => c.CustomerId == _activeCustomerId)!;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to get cart");
            }
            
        }
        
        return cart;
    }

    public bool CheckProductById(int id)
    {
        using (var context = new OnlineStoreContext())
        {
            Product product = context.Products.SingleOrDefault(p => p.ProductId == id)!;
            if (product == null)
                {
                    Console.WriteLine("Couldn't find product by specified productId");
                    return false;
                }
            else
                {
                    return true;
                }
        }
    }

    public bool DeleteProduct(int id)
    {
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Product product = context.Products.SingleOrDefault(p => p.ProductId == id)!;
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to delete product");
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
        
        Cart cart = GetActiveCustomerCart();
        
        using (var context = new OnlineStoreContext())
        {
            try
            {
                CartItem cartItem =
                    context.CartItems.SingleOrDefault(item =>
                        item.ProductId == productId && item.CartId == cart.CartId);
                if (cartItem == null)
                {
                    cartItem = new CartItem() { ProductId = productId, CartId = cart.CartId, Quantity = quantity };
                    context.CartItems.Add(cartItem);
                    context.SaveChanges();
                }
                else
                {
                    cartItem.Quantity = quantity;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to add to cart");
                return false;
            }
        }
    }

    public bool CheckCartItemById(int cartItemId)
    {
        Cart cart = GetActiveCustomerCart();

        if (cart.CartItems.SingleOrDefault(item => item.CartItemId == cartItemId) != null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    

    public bool DeleteCartItemById(int cartItemId)
    {
        if (!HasActiveCustomer())
        {
            return false;
        }
        using (var context = new OnlineStoreContext())
        {
            try
            {
                Cart cart = context.Carts.Include(c=> c.CartItems).SingleOrDefault(c => c.CustomerId == _activeCustomerId)!;
                if (cart == null)
                {
                    return false;
                }
                
                CartItem itemToDelete =
                    cart.CartItems.SingleOrDefault(item =>
                        item.CartItemId == cartItemId)
                    !; // we already check if carditem exists in cart in CheckCartItemById method
                context.CartItems.Remove(itemToDelete);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to delete cart");
                throw;
                
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
        Cart cart = GetActiveCustomerCart();
        
        
        Console.WriteLine(cart.CartItems.First().ProductId);
        using (var context = new OnlineStoreContext())
        {
            try
            {
                if (cart.CartItems.Count == 0)
                {
                    Console.WriteLine("Cart is empty");
                    return null;
                }

                var cartItemsToDelete = context.CartItems.Include(c => c.Product).Where(c => c.CartId == cart.CartId)
                    .ToList();
                context.CartItems.RemoveRange(cartItemsToDelete);
                

                order = new CustomerOrder()
                {
                    CustomerId = cart.CustomerId,
                    CustomerName = cart.Customer.FirstName + " " + cart.Customer.LastName,
                };

                foreach (var cartItem in cartItemsToDelete)
                {
                    OrderLine orderLine = new OrderLine()
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        ProductName = cartItem.Product.Name,
                        LineTotal = cartItem.Product!.Price * cartItem.Quantity,
                        UnitPrice = cartItem.Product.Price
                    };
                    order.OrderLines.Add(orderLine);
                }

                order.Date = DateTime.Now;
                order.Total = order.OrderLines.Sum(ol => ol.LineTotal);
                context.CustomerOrders.Add(order);
                context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error while trying to checkout");
            }
        }

        return order;

    }
    
    
    
    
    
    
}