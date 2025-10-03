namespace OnlineStoreClassLibrary;

public class CustomerOrder
{
    public int OrderId { get; private set; }             
    public DateTime Date { get; private set; }
    public decimal Total { get; set; }                  
    public string CustomerName { get; set; } = null!;    
    public int? CustomerId { get; set; }                 
    public Customer? Customer { get; set; }
    public List<OrderLine> Lines { get; } = new();

    public CustomerOrder(int orderId, DateTime date)
    {
        OrderId = orderId;
        Date = date;
    }

    public CustomerOrder(DateTime date)
    {
        Date = date;
    }
}