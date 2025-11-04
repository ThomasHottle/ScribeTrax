namespace ScribeTrax.Models
{
    public class SubmissionViewModel
    {
        public int SubmissionId { get; set; }

        public int? WorkId { get; set; }
        public string? WorkTitle { get; set; }
        public string? GenreName { get; set; }
        public string? BylineName { get; set; }

        public int? MarketId { get; set; }
        public string? MarketName { get; set; }
        public string? MarketType { get; set; }
        public string? EditorName { get; set; }

        public DateTime? SubmissionDate { get; set; }

        public bool IsAccepted { get; set; } // Optional: if you track status
        public bool IsPaid { get; set; }     // Optional: if linked to payments
    }
}