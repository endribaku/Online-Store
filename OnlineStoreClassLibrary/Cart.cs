namespace OnlineStoreClassLibrary;

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