using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class WorkCreateModel
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        public int? BylineId { get; set; }

        public int? GenreId { get; set; }
    }
}
