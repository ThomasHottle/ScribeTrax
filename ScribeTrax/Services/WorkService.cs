using Microsoft.EntityFrameworkCore;
using ScribeTrax.Models;
using ScribeTrax.Context;

namespace ScribeTrax.Services
{
    public class WorkService
    {
        private readonly ScribeTraxDbContext _context;

        public WorkService(ScribeTraxDbContext context)
        {
            _context = context;
        }



        public WorkViewModel GetWorkById(int id)
        {
            var work = _context.Works
                .Include(w => w.Byline)
                .Include(w => w.Genre)
                .FirstOrDefault(w => w.WorkId == id);

            if (work == null) return null;

            var submissions = _context.Submissions
                .Include(s => s.Market)
                .Where(s => s.WorkId == id)
                .OrderByDescending(s => s.SubmissionDate)
                .ToList();

            var lastSubmittedDate = submissions.FirstOrDefault()?.SubmissionDate;
            var mostRecentMarketName = submissions.FirstOrDefault()?.Market?.Name;

            var submissionCount = submissions.Count;
            var hasPayments = _context.Payments.Any(p => p.WorkId == id);

            return new WorkViewModel
            {
                WorkId = work.WorkId,
                Title = work.Title,
                Type = work.Type,
                BylineId = work.BylineId,
                BylineName = work.Byline?.Name,
                GenreId = work.GenreId,
                GenreName = work.Genre?.Name,
                SubmissionCount = submissionCount,
                HasPayments = hasPayments,
                LastSubmittedDate = lastSubmittedDate,
                MostRecentMarketName = mostRecentMarketName
            };
        }
    }
}
