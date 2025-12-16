using ScribeTrax.Models;

namespace ScribeTrax.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public int? WorkId { get; set; }
        public int? BylineId { get; set; }
        public int? MarketId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string Status { get; set; }
        public int? PaymentTypeId { get; set; }
        public decimal? Fee { get; set; }
        public bool? SelfPublished { get; set; }
        public bool? Royalty { get; set; }

        // Navigation properties
        public Work Work { get; set; }
        public Market Market { get; set; }
        public Byline Byline { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
