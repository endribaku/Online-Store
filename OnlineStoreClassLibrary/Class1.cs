namespace OnlineStoreClassLibrary
{
    public record ProductDto(int Id, string Name, decimal Price);
    public record CustomerDto(int Id, string Name);
    public record OrderDto(int Id, decimal Total, CustomerDto Customer, DateTime OrderDate);
    public record CartItemDto(int Id, string Name, decimal Price, int Quantity);

    
    public class Product
    {
        private static int _currentLastProductId;
        
        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(string name, decimal price)
        {
            this.ProductId = _currentLastProductId++;
            this.Name = name;
            this.Price = price;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Price: {Price.ToString("F2")}";
        }
    }
    
    public class CartItem
    {
        public Product Product{ get;}
        public int Quantity { get; set; }
        public CartItem(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }
    }

    public class Customer: IShoppable
    {
        private static int _currentLastCustomerId;
        public int CustomerId { get; private set; }
        public string Name { get; set; }
        
        private List<CartItem> _shoppingCart;

        public Customer(string name)
        {
            this.CustomerId = _currentLastCustomerId++;
            this.Name = name;
            this._shoppingCart = new List<CartItem>();
        }

        public void AddProduct(CartItem cartItem)
        {
            if (_shoppingCart.Find(x => x.Product.ProductId == cartItem.Product.ProductId) == null)
            {
                _shoppingCart.Add(cartItem);
            }
            else
            {
                _shoppingCart.Find(x => x.Product.ProductId == cartItem.Product.ProductId)!.Quantity = cartItem.Quantity;
            }
        }

        public void RemoveProduct(CartItem cartItem)
        {
            if (_shoppingCart.Find(x => x.Product.ProductId == cartItem.Product.ProductId) == null) return;
            
            _shoppingCart.Remove(_shoppingCart.Find(x => x.Product.ProductId == cartItem.Product.ProductId)!);
        }

        public Order Checkout()
        {
            if (_shoppingCart.Count == 0) return null!;
            Order order = new Order(this, new List<CartItem>(_shoppingCart));
            _shoppingCart.Clear();
            return order;
        }

        public List<CartItemDto> GetShoppingCart()
        {
            return _shoppingCart.Select(c => new CartItemDto(c.Product.ProductId,c.Product.Name ,c.Product.Price, c.Quantity)).ToList();
        }

        public override string ToString()
        {
            return $"CustomerId: {CustomerId}, Name: {Name}";
        }
        
    }

    public class Order
    {
        private static int _currentLastOrderId;
        
        public int OrderId { get; set; }
        public Customer Customer { get; private set; }
        public DateTime OrderDate { get; private set; }
        
        private readonly List<CartItem> _items;
        
        public Decimal OrderTotal {get; private set;}

        public Order(Customer customer, List<CartItem> items)
        {
            this.OrderId = _currentLastOrderId++;
            this.Customer = customer;
            this.OrderDate = DateTime.Now;
            this._items = items;
            this.OrderTotal = this._items.Sum(item => item.Product.Price * item.Quantity);
        }


        public override string ToString()
        {
            decimal orderTotalPrice =  this._items.Sum(c => c.Product.Price * c.Quantity);
            string customerName = this.Customer.Name;
            string orderProductInformation = this._items.Count.ToString();
            
            return "Customer: " + customerName + "\n" + orderProductInformation + "Order Total: " + orderTotalPrice;
        }
    }

    interface IShoppable
    {
        void AddProduct(CartItem item);
        void RemoveProduct(CartItem item);
        Order Checkout();
    }
}

