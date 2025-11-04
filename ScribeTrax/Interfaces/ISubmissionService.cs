using ScribeTrax.Models;

namespace ScribeTrax.Services
{
    public interface ISubmissionService
    {
        SubmissionViewModel GetSubmissionById(int id);
        IEnumerable<SubmissionViewModel> GetSubmissionsByWorkId(int workId);
        IEnumerable<SubmissionViewModel> GetAllSubmissions();

        // Optional: Add methods for creation, update, or deletion
         void CreateSubmission(Submission submission);
         void UpdateSubmission(Submission submission);
         void DeleteSubmission(int id);
    }
}