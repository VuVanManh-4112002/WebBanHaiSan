using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? Contents { get; set; }

    public string? Image { get; set; }

    public string? Decription { get; set; }

    public DateTime? Creatdate { get; set; }
}
