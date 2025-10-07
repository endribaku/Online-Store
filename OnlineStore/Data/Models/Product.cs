using System;
using System.Collections.Generic;

namespace OnlineStore;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
