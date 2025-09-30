using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;
public interface ICartRepository
{
    IEnumerable<Cart> GetCarts();
    Cart GetCartById(int cartId);
    void CreateCart(Cart cart);
    void UpdateCart(Cart cart);
    void DeleteCart(Cart cart);
}