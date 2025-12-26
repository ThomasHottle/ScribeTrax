using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;


namespace ScribeTrax.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }

        // Editable fields
        public int? WorkId { get; set; }
        public int? MarketId { get; set; }
        public int? SubmissionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentTypeId { get; set; }

        // Display-only fields
        [NotMapped][BindNever] public string? WorkTitle { get; set; }
        [NotMapped][BindNever] public string? MarketName { get; set; }
        [NotMapped][BindNever] public string? BylineName { get; set; }
        [NotMapped][BindNever] public string? SubmissionDate { get; set; }
        [NotMapped][BindNever] public string? PaymentTypeName { get; set; }
    }
}