using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Interfaces;

namespace ScribeTrax.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ScribeTraxDbContext _context;

        public SubmissionService(ScribeTraxDbContext context)
        {
            _context = context;
        }

        public SubmissionViewModel GetSubmissionById(int id)
        {
            var submission = _context.Submissions
                .Include(s => s.Work)
                    .ThenInclude(w => w.Byline)
                .Include(s => s.Work)
                    .ThenInclude(w => w.Genre)
                .Include(s => s.Market)
                .FirstOrDefault(s => s.SubmissionId == id);

            if (submission == null) return null;

            return new SubmissionViewModel
            {
                SubmissionId = submission.SubmissionId,
                WorkId = submission.WorkId,
                WorkTitle = submission.Work?.Title,
                GenreName = submission.Work?.Genre?.Name,
                BylineName = submission.Work?.Byline?.Name,
                MarketId = submission.MarketId,
                MarketName = submission.Market?.Name,
                MarketType = submission.Market?.Type,
                EditorName = submission.Market?.Editor,
                SubmissionDate = submission.SubmissionDate,
                IsAccepted = false, // Placeholder if you add status later
                IsPaid = _context.Payments.Any(p => p.WorkId == submission.WorkId)
            };
        }

        public IEnumerable<SubmissionViewModel> GetSubmissionsByWorkId(int workId)
        {
            return _context.Submissions
                .Include(s => s.Work)
                    .ThenInclude(w => w.Byline)
                .Include(s => s.Work)
                    .ThenInclude(w => w.Genre)
                .Include(s => s.Market)
                .Where(s => s.WorkId == workId)
                .OrderByDescending(s => s.SubmissionDate)
                .Select(s => new SubmissionViewModel
                {
                    SubmissionId = s.SubmissionId,
                    WorkId = s.WorkId,
                    WorkTitle = s.Work.Title,
                    GenreName = s.Work.Genre.Name,
                    BylineName = s.Work.Byline.Name,
                    MarketId = s.MarketId,
                    MarketName = s.Market.Name,
                    MarketType = s.Market.Type,
                    EditorName = s.Market.Editor,
                    SubmissionDate = s.SubmissionDate,
                    IsAccepted = false,
                    IsPaid = _context.Payments.Any(p => p.WorkId == s.WorkId)
                })
                .ToList();
        }

        public IEnumerable<SubmissionViewModel> GetAllSubmissions()
        {
            return _context.Submissions
                .Include(s => s.Work)
                    .ThenInclude(w => w.Byline)
                .Include(s => s.Work)
                    .ThenInclude(w => w.Genre)
                .Include(s => s.Market)
                .OrderByDescending(s => s.SubmissionDate)
                .Select(s => new SubmissionViewModel
                {
                    SubmissionId = s.SubmissionId,
                    WorkId = s.WorkId,
                    WorkTitle = s.Work.Title,
                    GenreName = s.Work.Genre.Name,
                    BylineName = s.Work.Byline.Name,
                    MarketId = s.MarketId,
                    MarketName = s.Market.Name,
                    MarketType = s.Market.Type,
                    EditorName = s.Market.Editor,
                    SubmissionDate = s.SubmissionDate,
                    IsAccepted = false,
                    IsPaid = _context.Payments.Any(p => p.WorkId == s.WorkId)
                })
                .ToList();
        }
        public void CreateSubmission(Submission submission)
        {
            _context.Submissions.Add(submission);
            _context.SaveChanges();
        }

        public void UpdateSubmission(Submission submission)
        {
            _context.Submissions.Update(submission);
            _context.SaveChanges();
        }

        public void DeleteSubmission(int id)
        {
            var submission = _context.Submissions.Find(id);
            if (submission != null)
            {
                _context.Submissions.Remove(submission);
                _context.SaveChanges();
            }
        }
    }
}