namespace OnlineStore.Data.Repositories.Interfaces;

public interface ICartRepository
{
    IEnumerable<Cart> GetCarts();
    Cart GetCartById(int customerId);
    void CreateCart(Cart cart);
    void UpdateCart(Cart cart);
    void DeleteCart(Cart cart);
}