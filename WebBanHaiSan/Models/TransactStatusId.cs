using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class TransactStatusId
{
    public int StatusId { get; set; }

    public string? Status { get; set; }

    public string? Decription { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
