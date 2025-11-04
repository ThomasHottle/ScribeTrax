using ScribeTrax.Models;

namespace ScribeTrax.Interfaces
{
    public interface IWorkService
    {
            WorkViewModel GetWorkById(int id);

            IEnumerable<WorkViewModel> GetAllWorks();

            // Optional: Add methods for creation, update, or deletion
            void CreateWork(Work work);
            void UpdateWork(Work work);
            void DeleteWork(int id);
    }
}
