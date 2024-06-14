using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public string? CartName { get; set; }

    public string? Title { get; set; }

    public string? Cover { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
