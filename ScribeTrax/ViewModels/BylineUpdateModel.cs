using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class BylineUpdateModel
    {
        [StringLength(100)]
        public string Name { get; set; }

        [RegularExpression("Author|Co-Author|Ghost")]
        public string Type { get; set; }

        public bool? IsInactive { get; set; }
    }
}
