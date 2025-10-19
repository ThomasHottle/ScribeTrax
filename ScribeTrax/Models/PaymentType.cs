using System;
using System.Collections.Generic;

namespace ScribeTrax.Models;

public partial class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string Name { get; set; } = null!;
}
