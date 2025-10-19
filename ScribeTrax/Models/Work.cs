using System;
using System.Collections.Generic;

namespace ScribeTrax.Models;

public partial class Work
{
    public int WorkId { get; set; }

    public int BylineId { get; set; }

    public string Title { get; set; } = null!;

    public string? Type { get; set; }

    public int? GenreId { get; set; }
}
