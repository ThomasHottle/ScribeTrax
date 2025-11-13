public class PaymentViewModel
{
    public int PaymentId { get; set; }
    public int? WorkId { get; set; }
    public string? WorkTitle { get; set; }
    public int? MarketId { get; set; }
    public string? MarketName { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public int? PaymentTypeId { get; set; }
    public string? PaymentTypeName { get; set; }
}