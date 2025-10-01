namespace OnlineStoreClassLibrary
{
    public record ProductDto(int Id, string Name, decimal Price);

    public record CustomerDto(int Id, string Name);

    public record OrderDto(int Id, decimal Total, CustomerDto Customer, DateTime OrderDate);

    public record CartItemDto(int Id, string Name, decimal Price, int Quantity);
    
    public class Customer
    {
        public int CustomerId { get; set; }       
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        
        public Cart? Cart { get; set; }                      
        public List<CustomerOrder> Orders { get; } = new();
    }


    public class Product
    {
        public int ProductId { get; private set; }           
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        
        
        public List<OrderLine> OrderLines { get; } = new();
        public List<CartItem> CartItems { get; } = new();

        public Product(int productId)
        {
            ProductId = productId;
        }

        public Product()
        {
            
        }
    }

    
    public class CustomerOrder
    {
        public int OrderId { get; private set; }             
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public decimal Total { get; set; }                  
        public string CustomerName { get; set; } = null!;    
        public int? CustomerId { get; set; }                 
        public Customer? Customer { get; set; }

        
        public List<OrderLine> Lines { get; } = new();
    }

   
    public class OrderLine
    {
        public int OrderLineId { get; private set; }         
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string ProductName { get; set; } = null!;     

        public int? ProductId { get; set; }                  
        public Product? Product { get; set; }

        public int OrderId { get; set; }
        public CustomerOrder Order { get; set; } = null!;
    }

    
    public class Cart
    {
        public int CartId { get; private set; }              
        public int CustomerId { get; set; }                  
        public Customer Customer { get; set; } = null!;
        
        public List<CartItem> Items { get; set; } = new();

        public Cart(int id)
        {
            CartId = id;
        }
        
        public Cart() {}
    }

    
    public class CartItem
    {
        public int CartItemId { get; private set; }          
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public CartItem(int id)
        {
            CartId = id;
        }

        public CartItem()
        {
            
        }
    }

    public class Order
    {
        public int OrderId { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string CustomerName { get; set; } = null!;
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }


}

