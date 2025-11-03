using System.ComponentModel.DataAnnotations;

public class WorkViewModel
{
    public int WorkId { get; set; }

    [Required]
    [StringLength(500)]
    public string Title { get; set; }

    [Display(Name = "Work Type")]
    public string Type { get; set; }

    public int BylineId { get; set; }

    [Display(Name = "Author")]
    public string BylineName { get; set; }

    public int? GenreId { get; set; }

    [Display(Name = "Genre")]
    public string GenreName { get; set; }

    public int SubmissionCount { get; set; }

    public bool HasPayments { get; set; }

    [Display(Name = "Last Submitted")]
    public DateTime? LastSubmittedDate { get; set; }

    [Display(Name = "Most Recent Market")]
    public string MostRecentMarketName { get; set; }
}