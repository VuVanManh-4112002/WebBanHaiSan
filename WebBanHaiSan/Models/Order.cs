using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int? StatusId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentId { get; set; }

    public string? ShipDetail { get; set; }

    public int? TotalMoney { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual TransactStatusId? Status { get; set; }
}
