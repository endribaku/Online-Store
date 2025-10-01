using OnlineStoreClassLibrary;

namespace OnlineStore.Data.Repositories.Interfaces;
using OnlineStoreClassLibrary;
public interface ICartItemRepository
{
    List<CartItem> GetCartItems(int cartId);
    CartItem GetCartItemById(int cartItemId, int productId);
    void CreateCartItem(CartItem cartItem);
    void UpdateCartItem(CartItem cartItem);
    void DeleteCartItem(CartItem cartItem);
    
}