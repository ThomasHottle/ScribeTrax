using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Interfaces;
using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Services
{
    public class WorkService : IWorkService
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

        public IEnumerable<WorkViewModel> GetAllWorks()
        {
            return _context.Works
                .Include(w => w.Byline)
                .Include(w => w.Genre)
                .OrderBy(w => w.Title)
                .Select(work => new WorkViewModel
                {
                    WorkId = work.WorkId,
                    Title = work.Title,
                    Type = work.Type,
                    BylineId = work.BylineId,
                    BylineName = work.Byline.Name,
                    GenreId = work.GenreId,
                    GenreName = work.Genre.Name,
                    SubmissionCount = _context.Submissions.Count(s => s.WorkId == work.WorkId),
                    HasPayments = _context.Payments.Any(p => p.WorkId == work.WorkId),
                    LastSubmittedDate = _context.Submissions
                        .Where(s => s.WorkId == work.WorkId)
                        .OrderByDescending(s => s.SubmissionDate)
                        .Select(s => s.SubmissionDate)
                        .FirstOrDefault(),
                    MostRecentMarketName = _context.Submissions
                        .Where(s => s.WorkId == work.WorkId)
                        .OrderByDescending(s => s.SubmissionDate)
                        .Select(s => s.Market.Name)
                        .FirstOrDefault()
                })
                .ToList();
        }

        public IEnumerable<Byline> GetAllBylines()
        {
            return _context.Bylines
                .Where(b => !b.Inactive.HasValue || !b.Inactive.Value)
                .OrderBy(b => b.Name)
                .ToList();
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return _context.Genres
                .OrderBy(g => g.Name)
                .ToList();
        }



        public void CreateWork(WorkCreateModel model)
        {
            var work = new Work
            {
                Title = model.Title,
                GenreId = model.GenreId,
                BylineId = model.BylineId.Value,
            };

            _context.Works.Add(work);
            _context.SaveChanges();
        }



        public void UpdateWork(Work work)
        {
            var existing = _context.Works.Find(work.WorkId);
            if (existing == null) return;

            existing.Title = work.Title;
            existing.Type = work.Type;
            existing.BylineId = work.BylineId;
            existing.GenreId = work.GenreId;

            _context.SaveChanges();
        }

        public void DeleteWork(int id)
        {
            var work = _context.Works.Find(id);
            if (work != null)
            {
                _context.Works.Remove(work);
                _context.SaveChanges();
            }
        }
    }
}