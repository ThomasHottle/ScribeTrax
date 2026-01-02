using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Interfaces
{
    public interface IWorkService
    {
            WorkViewModel GetWorkById(int id);

            IEnumerable<WorkViewModel> GetAllWorks();

            IEnumerable<Byline> GetAllBylines();
            IEnumerable<Genre> GetAllGenres();

            // Optional: Add methods for creation, update, or deletion
            void CreateWork(WorkCreateModel work);
            void UpdateWork(Work work);
            void DeleteWork(int id);
    }
}
