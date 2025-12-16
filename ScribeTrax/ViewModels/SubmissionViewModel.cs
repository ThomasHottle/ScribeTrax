using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScribeTrax.ViewModels
{ 
    public class SubmissionViewModel
    {
        public int SubmissionId { get; set; }
        public int WorkId { get; set; }
        public int? BylineId { get; set; }
        public int MarketId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string Status { get; set; }
        public int? PaymentTypeId { get; set; }
        public decimal? Fee { get; set; }
        public bool? SelfPublished { get; set; }
        public bool? Royalty { get; set; }
        // Display-only fields
        [NotMapped]
        [BindNever]
        public string? WorkTitle { get; set; }

        [NotMapped]
        [BindNever]
        public string? BylineName { get; set; }

        [NotMapped]
        [BindNever]
        public string? MarketName { get; set; }


    }
}