using System;
using System.Collections.Generic;

namespace ScribeTrax.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? WorkId { get; set; }

    public int? MarketId { get; set; }

    public DateOnly? PaymentDate { get; set; }
}
