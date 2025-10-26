using ScribeTrax.ViewModels;

namespace ScribeTrax.Interfaces
{
    public interface IBylineService
    {
        /// <summary>
        /// Retrieves all bylines, optionally including inactive ones.
        /// </summary>
        IEnumerable<BylineViewModel> GetAllBylines(bool includeInactive = false);

        /// <summary>
        /// Retrieves a single byline by its ID.
        /// </summary>
        BylineViewModel GetBylineById(int id);

        /// <summary>
        /// Creates a new byline.
        /// </summary>
        void CreateByline(BylineCreateModel model);

        /// <summary>
        /// Updates an existing byline.
        /// </summary>
        void UpdateByline(int id, BylineUpdateModel model);

        /// <summary>
        /// Marks a byline as inactive.
        /// </summary>
        void DeactivateByline(int id);
    }
}
