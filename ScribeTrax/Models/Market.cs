using System;
using System.Collections.Generic;

namespace ScribeTrax.Models;

public partial class Market
{
    public int MarketId { get; set; }

    public string Name { get; set; } = null!;

    public string? Editor { get; set; }

    public string? Type { get; set; }

    public string? Email { get; set; }

    public string? Url { get; set; }

    public string? Postal { get; set; }
}
