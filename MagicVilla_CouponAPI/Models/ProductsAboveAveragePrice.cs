using System;
using System.Collections.Generic;

namespace MagicVilla_CouponAPI.Models;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
