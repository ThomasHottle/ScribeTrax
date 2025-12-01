using System.ComponentModel.DataAnnotations;

namespace ScribeTrax.ViewModels
{
    public class WorkUpdateModel
    {
        public int WorkId { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public string Type { get; set; }

        public int BylineId { get; set; }

        public int GenreId { get; set; }
    }
}
