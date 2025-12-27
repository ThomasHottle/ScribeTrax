using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Interfaces;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;
        private readonly ScribeTraxDbContext _context;

        public PaymentController(IPaymentService service, ScribeTraxDbContext context)
        {
            _service = service;
            _context = context;
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
            ViewBag.Works = new SelectList(_context.Works, "WorkId", "Title");
            ViewBag.Markets = new SelectList(_context.Markets, "MarketId", "Name");
            ViewBag.Submissions = new SelectList(_context.Submissions, "SubmissionId", "SubmissionDate");
            ViewBag.PaymentTypes = new SelectList(_context.PaymentTypes, "PaymentTypeId", "Name");


            return View(new PaymentViewModel());
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Works = new SelectList(_context.Works, "WorkId", "Title");
                ViewBag.Markets = new SelectList(_context.Markets, "MarketId", "Name");
                ViewBag.Submissions = new SelectList(_context.Submissions, "SubmissionId", "SubmissionDate");
                ViewBag.PaymentTypes = new SelectList(_context.PaymentTypes, "PaymentTypeId", "Name");


                return View(model);
            }

            _service.CreatePayment(model);
            return RedirectToAction(nameof(Index));
        }



        // GET: Payment/Edit/5
        public IActionResult Edit(int id)
        {
            var payment = _service.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            ViewBag.Works = new SelectList(_context.Works, "WorkId", "Title", payment.WorkId);
            ViewBag.Markets = new SelectList(_context.Markets, "MarketId", "Name", payment.MarketId);
            ViewBag.Submissions = new SelectList(_context.Submissions, "SubmissionId", "SubmissionDate", payment.SubmissionId);
            ViewBag.PaymentTypes = new SelectList(_context.PaymentTypes, "PaymentTypeId", "Name", payment.PaymentTypeId);

            return View(payment);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Works = new SelectList(_context.Works, "WorkId", "Title", model.WorkId);
                ViewBag.Markets = new SelectList(_context.Markets, "MarketId", "Name", model.MarketId);
                ViewBag.Submissions = new SelectList(_context.Submissions, "SubmissionId", "SubmissionDate", model.SubmissionId);
                ViewBag.PaymentTypes = new SelectList(_context.PaymentTypes, "PaymentTypeId", "Name", model.PaymentTypeId);

                return View(model);
            }

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