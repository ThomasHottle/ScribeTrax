using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class BylineUpdateModel
    {
        public int BylineId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [RegularExpression("Author|Co-Author|Ghost")]
        public string Type { get; set; }

        public bool? IsInactive { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> TypeOptions { get; set; }
    }
}
