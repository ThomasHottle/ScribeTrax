using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Interfaces;
using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ScribeTraxDbContext _context;

        public PaymentService(ScribeTraxDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PaymentViewModel> GetAllPayments()
        {
            return _context.Payments
                .Include(p => p.Work)
                .Include(p => p.Market)
                .Include(p => p.Submission)
                    .ThenInclude(s => s.Byline)
                .Include(p => p.PaymentType)
                .Select(p => new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    WorkId = p.WorkId,
                    MarketId = p.MarketId,
                    SubmissionId = p.SubmissionId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentTypeId = p.PaymentTypeId,

                    // Display fields
                    WorkTitle = p.Work != null ? p.Work.Title : null,
                    MarketName = p.Market != null ? p.Market.Name : null,
                    BylineName = p.Submission != null ? p.Submission.Byline.Name : null,
                    SubmissionDate = p.Submission != null ? p.Submission.SubmissionDate.ToString() : null,
                    PaymentTypeName = p.PaymentType != null ? p.PaymentType.Name : null
                })
                .ToList();
        }

        public PaymentViewModel? GetPaymentById(int id)
        {
            var p = _context.Payments
                .Include(p => p.Work)
                .Include(p => p.Market)
                .Include(p => p.Submission)
                    .ThenInclude(s => s.Byline)
                .Include(p => p.PaymentType)
                .FirstOrDefault(p => p.PaymentId == id);

            if (p == null)
                return null;

            return new PaymentViewModel
            {
                PaymentId = p.PaymentId,
                WorkId = p.WorkId,
                MarketId = p.MarketId,
                SubmissionId = p.SubmissionId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentTypeId = p.PaymentTypeId,

                WorkTitle = p.Work?.Title,
                MarketName = p.Market?.Name,
                BylineName = p.Submission?.Byline?.Name,
                SubmissionDate = p.Submission?.SubmissionDate.ToString(),
                PaymentTypeName = p.PaymentType?.Name
            };
        }

        public void CreatePayment(PaymentViewModel model)
        {
            var entity = new Payment
            {
                WorkId = model.WorkId,
                MarketId = model.MarketId,
                SubmissionId = model.SubmissionId,
                Amount = model.Amount,
                PaymentDate = model.PaymentDate,
                PaymentTypeId = model.PaymentTypeId
            };

            _context.Payments.Add(entity);
            _context.SaveChanges();
        }

        public void UpdatePayment(PaymentViewModel model)
        {
            var entity = _context.Payments.FirstOrDefault(p => p.PaymentId == model.PaymentId);
            if (entity == null)
                return;

            entity.WorkId = model.WorkId;
            entity.MarketId = model.MarketId;
            entity.SubmissionId = model.SubmissionId;
            entity.Amount = model.Amount;
            entity.PaymentDate = model.PaymentDate;
            entity.PaymentTypeId = model.PaymentTypeId;

            _context.SaveChanges();
        }

        public void DeletePayment(int id)
        {
            var entity = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (entity == null)
                return;

            _context.Payments.Remove(entity);
            _context.SaveChanges();
        }
    }
}