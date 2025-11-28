using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class BylineViewModel
    {
        public int BylineId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Type { get; set; } // Consider enum or dropdown binding

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }
        // ✅ Navigation property for Details view
        public List<WorkViewModel>? Works { get; set; }
    }
}
