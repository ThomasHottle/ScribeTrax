using System;
using System.Collections.Generic;

namespace ScribeTrax.Models;

public partial class Byline
{
    public int BylineId { get; set; }

    public string Name { get; set; }

    public string? Type { get; set; }

    public bool? Inactive { get; set; }
}
