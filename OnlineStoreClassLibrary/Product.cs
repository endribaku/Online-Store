namespace OnlineStoreClassLibrary;

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
