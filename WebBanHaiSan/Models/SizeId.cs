using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class SizeId
{
    public string Size { get; set; } = null!;

    public string? Decription { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
