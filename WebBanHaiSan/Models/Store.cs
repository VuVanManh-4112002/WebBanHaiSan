using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? Email { get; set; }

    public int? Phone { get; set; }

    public string? Adress { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
