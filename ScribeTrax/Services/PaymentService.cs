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
                .Select(p => new PaymentViewModel
                {
                    PaymentId = p.PaymentId,
                    WorkId = p.WorkId,
                    WorkTitle = p.Work != null ? p.Work.Title : null,
                    MarketId = p.MarketId,
                    MarketName = p.Market != null ? p.Market.Name : null,
                    PaymentDate = p.PaymentDate,
                    PaymentTypeId = p.PaymentTypeId,
                    PaymentTypeName = p.PaymentType != null ? p.PaymentType.Name : null
                })
                .ToList();
        }

        public PaymentViewModel GetPaymentById(int id)
        {
            var p = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (p == null) return null;

            return new PaymentViewModel
            {
                PaymentId = p.PaymentId,
                WorkId = p.WorkId,
                WorkTitle = p.Work?.Title,
                MarketId = p.MarketId,
                MarketName = p.Market?.Name,
                PaymentDate = p.PaymentDate,
                PaymentTypeId = p.PaymentTypeId,
                PaymentTypeName = p.PaymentType?.Name
            };
        }

        public void CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public void UpdatePayment(Payment payment)
        {
            _context.Payments.Update(payment);
            _context.SaveChanges();
        }

        public void DeletePayment(int id)
        {
            var payment = _context.Payments.Find(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                _context.SaveChanges();
            }
        }
    }
}