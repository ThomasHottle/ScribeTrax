using ScribeTrax.Models;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Interfaces
{
    public interface IPaymentService
    {
        IEnumerable<PaymentViewModel> GetAllPayments();
        PaymentViewModel? GetPaymentById(int id);

        void CreatePayment(PaymentViewModel model);
        void UpdatePayment(PaymentViewModel model);
        void DeletePayment(int id);
    }
}