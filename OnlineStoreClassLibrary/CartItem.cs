namespace OnlineStoreClassLibrary;

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