using System;
using System.Collections.Generic;

namespace OnlineStore;

public partial class CustomerOrder
{
    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public decimal Total { get; set; }

    public string CustomerName { get; set; } = null!;

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
