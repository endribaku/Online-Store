using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreLibrary;
public interface ICartItemRepository
{
    IEnumerable<CartItem> GetCartItems();
    CartItem GetCartItemById(int cartId);
    void CreateCartItem(Cart cart);
    void UpdateCartItem(Cart cart);
    void DeleteCartItem(Cart cart);
    
}