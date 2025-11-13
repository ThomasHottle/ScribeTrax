using ScribeTrax.Models;

public partial class Payment
{
    public int PaymentId { get; set; }
    public int? WorkId { get; set; }
    public int? MarketId { get; set; }
    public DateOnly? PaymentDate { get; set; }

    public int? PaymentTypeId { get; set; } // ✅ New foreign key

    public virtual Work? Work { get; set; }
    public virtual Market? Market { get; set; }
    public virtual PaymentType? PaymentType { get; set; } // ✅ Navigation property
}