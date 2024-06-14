using System;
using System.Collections.Generic;

namespace WebBanHaiSan.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Rolename { get; set; }

    public string? Decription { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
