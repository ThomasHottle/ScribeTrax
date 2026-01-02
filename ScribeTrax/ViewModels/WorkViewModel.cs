using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class WorkViewModel
    {
        public int? WorkId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Display(Name = "Work Type")]
        public string Type { get; set; }

        public int? BylineId { get; set; }

        [Display(Name = "Author")]
        public string BylineName { get; set; }

        public int? GenreId { get; set; }

        [Display(Name = "Genre")]
        public string GenreName { get; set; }

        [ValidateNever]
        public int SubmissionCount { get; set; }

        [ValidateNever]
        public bool HasPayments { get; set; }

        [ValidateNever]
        [Display(Name = "Last Submitted")]
        public DateTime? LastSubmittedDate { get; set; }

        [ValidateNever]
        [Display(Name = "Most Recent Market")]
        public string MostRecentMarketName { get; set; }
    }
}