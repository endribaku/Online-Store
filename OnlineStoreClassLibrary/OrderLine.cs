namespace OnlineStoreClassLibrary;

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

    public OrderLine(int orderLineId)
    {
        OrderId = orderLineId;
    }

    public OrderLine()
    {
            
    }
}