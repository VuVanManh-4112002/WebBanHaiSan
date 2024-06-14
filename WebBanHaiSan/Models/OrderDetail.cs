using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public int? Total { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
