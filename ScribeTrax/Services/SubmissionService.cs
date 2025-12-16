using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using System.Collections.Generic;
using System.Linq;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ScribeTraxDbContext _context;

        public SubmissionService(ScribeTraxDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SubmissionViewModel> GetAllSubmissions()
        {
            return _context.Submissions
                .Include(s => s.Work)
                .Include(s => s.Market)
                .Include(s => s.Byline)
                .OrderByDescending(s => s.SubmissionDate)
                .Select(s => new SubmissionViewModel
                {
                    SubmissionId = s.SubmissionId,
                    WorkId = s.WorkId ?? 0,
                    BylineId = s.BylineId,
                    MarketId = s.MarketId ?? 0,
                    SubmissionDate = s.SubmissionDate,
                    Status = s.Status,
                    Fee = s.Fee,
                    SelfPublished = s.SelfPublished,
                    Royalty = s.Royalty,

                    // Names for display
                    WorkTitle = s.Work != null ? s.Work.Title : string.Empty,
                    BylineName = s.Byline != null ? s.Byline.Name : string.Empty,
                    MarketName = s.Market != null ? s.Market.Name : string.Empty
                })
                .ToList();
        }
        public SubmissionViewModel GetSubmissionById(int id)
        {
            var s = _context.Submissions
                .Include(x => x.Work)
                .Include(x => x.Market)
                .Include(x => x.Byline)
                .Include(x => x.PaymentType)
                .FirstOrDefault(x => x.SubmissionId == id);

            if (s == null) return null;

            return new SubmissionViewModel
            {
                SubmissionId = s.SubmissionId,
                WorkId = s.WorkId ?? 0,
                MarketId = s.MarketId ?? 0,
                BylineId = s.BylineId,
                SubmissionDate = s.SubmissionDate,
                Status = s.Status,
                Fee = s.Fee,
                SelfPublished = s.SelfPublished,
                Royalty = s.Royalty
            };
        }
        // Existing methods (GetSubmissionById, GetAllSubmissions, etc.) …

        public void CreateSubmission(SubmissionViewModel model)
        {
            var entity = new Submission
            {
                WorkId = model.WorkId,
                MarketId = model.MarketId,
                BylineId = model.BylineId,
                SubmissionDate = model.SubmissionDate,
                Status = model.Status,
                PaymentTypeId = model.PaymentTypeId,
                Fee = model.Fee,
                SelfPublished = model.SelfPublished,
                Royalty = model.Royalty
            };

            _context.Submissions.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateSubmission(SubmissionViewModel model)
        {
            var entity = _context.Submissions.FirstOrDefault(s => s.SubmissionId == model.SubmissionId);
            if (entity == null) return;

            entity.WorkId = model.WorkId;
            entity.MarketId = model.MarketId;
            entity.BylineId = model.BylineId;
            entity.SubmissionDate = model.SubmissionDate;
            entity.Status = model.Status;
            entity.Fee = model.Fee;
            entity.SelfPublished = model.SelfPublished;
            entity.Royalty = model.Royalty;

            _context.SaveChanges();
        }

        public void DeleteSubmission(int id)
        {
            var entity = _context.Submissions.FirstOrDefault(s => s.SubmissionId == id);
            if (entity == null) return;

            _context.Submissions.Remove(entity);
            _context.SaveChanges();
        }

        // ✅ New helper methods
        public IEnumerable<Work> GetWorks()
        {
            return _context.Works
                .OrderBy(w => w.Title)
                .ToList();
        }

        public IEnumerable<SubmissionViewModel> GetSubmissionsByWorkId(int workId)
        {           
            return _context.Submissions
            .Include(s => s.Work)
            .Include(s => s.Market)
            .Include(s => s.Byline)
            .Include(s => s.PaymentType)
            .Where(s => s.WorkId == workId)
            .OrderByDescending(s => s.SubmissionDate)
            .Select(s => new SubmissionViewModel
            {
                SubmissionId = s.SubmissionId,
                WorkId = s.WorkId ?? 0,
                MarketId = s.MarketId ?? 0,
                BylineId = s.BylineId,
                SubmissionDate = s.SubmissionDate,
                Status = s.Status,
                Fee = s.Fee,
                SelfPublished = s.SelfPublished,
                Royalty = s.Royalty
            })
            .ToList();
        }



        public IEnumerable<Market> GetMarkets()
        {
            return _context.Markets
                .OrderBy(m => m.Name)
                .ToList();
        }

        public IEnumerable<Byline> GetBylines()
        {
            return _context.Bylines
                .OrderBy(b => b.Name)
                .ToList();
        }
    }
}