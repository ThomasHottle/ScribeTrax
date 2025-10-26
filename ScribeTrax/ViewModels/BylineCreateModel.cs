using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class BylineCreateModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression("Author|Co-Author|Ghost", ErrorMessage = "Type must be Author, Co-Author, or Ghost.")]
        public string Type { get; set; }

        public bool IsInactive { get; set; } = false;
    }
}
