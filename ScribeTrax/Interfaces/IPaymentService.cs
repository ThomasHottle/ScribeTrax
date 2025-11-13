using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Interfaces
{
    public interface IPaymentService
    {
        IEnumerable<PaymentViewModel> GetAllPayments();
        PaymentViewModel GetPaymentById(int id);
        void CreatePayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int id);
    }
}