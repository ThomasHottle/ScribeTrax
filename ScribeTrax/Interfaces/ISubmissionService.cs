using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Services
{
    public interface ISubmissionService
    {
        SubmissionViewModel GetSubmissionById(int id);
        IEnumerable<SubmissionViewModel> GetAllSubmissions();
        IEnumerable<SubmissionViewModel> GetSubmissionsByWorkId(int workId);

        void CreateSubmission(SubmissionViewModel model);
        void UpdateSubmission(SubmissionViewModel model);
        void DeleteSubmission(int id);

        // New helpers for dropdowns
        IEnumerable<Work> GetWorks();
        IEnumerable<Market> GetMarkets();
        IEnumerable<Byline> GetBylines();
    }
}