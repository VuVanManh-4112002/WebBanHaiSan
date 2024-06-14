using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Productname { get; set; }

    public int CartId { get; set; }

    public string? Origin { get; set; }

    public string? Size { get; set; }

    public string? StatusPd { get; set; }

    public int? Price { get; set; }

    public double? Discount { get; set; }

    public string? Thumb { get; set; }

    public string? Video { get; set; }

    public bool Homeflag { get; set; }

    public bool Bestseller { get; set; }

    public string? Title { get; set; }

    public int? Stock { get; set; }

    public int StoreId { get; set; }

    public virtual Cart? Cart { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual SizeId? SizeNavigation { get; set; } = null;

    public virtual Store? Store { get; set; } = null!;

}
