namespace OnlineStoreClassLibrary;

public class Customer
{
    public int CustomerId { get; set; }       
    public string FirstName { get; set; }
    public string LastName  { get; set; }
        
    public Cart? Cart { get; set; }                      
    public List<CustomerOrder> Orders { get; } = new();
}