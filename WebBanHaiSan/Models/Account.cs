using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int? Phone { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool Active { get; set; }

    public string? Fullname { get; set; }

    public int RoleId { get; set; }

    public virtual Role? Role { get; set; } = null!;
}
