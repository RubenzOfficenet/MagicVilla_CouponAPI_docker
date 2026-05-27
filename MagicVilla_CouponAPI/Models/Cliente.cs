using System;
using System.Collections.Generic;

namespace MagicVilla_CouponAPI.Models;

public partial class Cliente
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? ContactTitle { get; set; }
}
