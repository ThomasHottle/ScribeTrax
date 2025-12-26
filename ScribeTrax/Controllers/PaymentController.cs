using Microsoft.AspNetCore.Mvc;
using ScribeTrax.Interfaces;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        // GET: Payment
        public IActionResult Index()
        {
            var payments = _service.GetAllPayments();
            return View(payments);
        }

        // GET: Payment/Details/5
        public IActionResult Details(int id)
        {
            var payment = _service.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create()
        {
            return View(new PaymentViewModel());
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.CreatePayment(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Edit/5
        public IActionResult Edit(int id)
        {
            var payment = _service.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.UpdatePayment(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Delete/5
        public IActionResult Delete(int id)
        {
            var payment = _service.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeletePayment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}