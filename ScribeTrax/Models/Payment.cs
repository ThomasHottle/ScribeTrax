using ScribeTrax.Models;

public class Payment
{
    public int PaymentId { get; set; }

    public int? WorkId { get; set; }
    public int? MarketId { get; set; }
    public int? SubmissionId { get; set; }

    public decimal Amount { get; set; }
    public DateTime? PaymentDate { get; set; }

    public int? PaymentTypeId { get; set; }

    // Navigation properties
    public Work Work { get; set; }
    public Market Market { get; set; }
    public Submission Submission { get; set; }
    public PaymentType PaymentType { get; set; }
}

