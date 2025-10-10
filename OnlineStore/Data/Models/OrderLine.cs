using System;
using System.Collections.Generic;

namespace OnlineStore;


public partial class OrderLine
{
    public int OrderLineId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public string ProductName { get; set; } = null!;

    public int? ProductId { get; set; }

    public int OrderId { get; set; }

    public virtual CustomerOrder Order { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
