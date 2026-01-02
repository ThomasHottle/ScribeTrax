using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Interfaces;
using ScribeTrax.Models;
using ScribeTrax.Services;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;
        private readonly ISubmissionService _submissionService;
        private readonly IWorkService _workService;
        private readonly IMarketService _marketService;
        private readonly ScribeTraxDbContext _context;

        public PaymentController(
        IPaymentService paymentService,
        ISubmissionService submissionService,
        IWorkService workService,
        IMarketService marketService)
        {
            _service = paymentService;
            _submissionService = submissionService;
            _workService = workService;
            _marketService = marketService;
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
        [HttpGet]
        public IActionResult Create(PaymentCreateRequest request)
        {
            ModelState.Clear();

            // Submissions
            var submissions = _submissionService.GetAllSubmissions().ToList();
            submissions.Insert(0, new SubmissionViewModel { SubmissionId = null, WorkTitle = "" });
            ViewBag.Submissions = new SelectList(submissions, "SubmissionId", "WorkTitle", request.SubmissionId);

            // Works
            var works = _workService.GetAllWorks().ToList();
            works.Insert(0, new WorkViewModel { WorkId = null, Title = "" });
            ViewBag.Works = new SelectList(works, "WorkId", "Title");

            // Markets
            var markets = _marketService.GetAllMarkets().ToList();
            markets.Insert(0, new MarketViewModel { MarketId = null, Name = "" });
            ViewBag.Markets = new SelectList(markets, "MarketId", "Name");

            // Payment Types
            var paymentTypes = new List<PaymentTypeOption>
{
                new PaymentTypeOption { Id = 0, Name = "" },
                new PaymentTypeOption { Id = 1, Name = "Flat Fee" },
                new PaymentTypeOption { Id = 2, Name = "Royalty" },
                new PaymentTypeOption { Id = 3, Name = "Advance" },
                new PaymentTypeOption { Id = 4, Name = "Revenue Share" },
                new PaymentTypeOption { Id = 5, Name = "Unpaid" },
                new PaymentTypeOption { Id = 6, Name = "Self-Published" }
            };

            ViewBag.PaymentTypes = new SelectList(paymentTypes, "Id", "Name");

            ViewBag.PaymentTypes = new SelectList(paymentTypes, "Id", "Name");

            return View(new PaymentViewModel
            {
                SubmissionId = request.SubmissionId
            });
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var submissions = _submissionService.GetAllSubmissions().ToList();
                submissions.Insert(0, new SubmissionViewModel { SubmissionId = null, WorkTitle = "" });

                var paymentTypes = new List<PaymentTypeOption>
                {
                    new PaymentTypeOption { Id = 0, Name = "" },
                    new PaymentTypeOption { Id = 1, Name = "Flat Fee" },
                    new PaymentTypeOption { Id = 2, Name = "Royalty" },
                    new PaymentTypeOption { Id = 3, Name = "Advance" },
                    new PaymentTypeOption { Id = 4, Name = "Revenue Share" },
                    new PaymentTypeOption { Id = 5, Name = "Unpaid" },
                    new PaymentTypeOption { Id = 6, Name = "Self-Published" }
                };

                ViewBag.Submissions = new SelectList(submissions, "SubmissionId", "WorkTitle", model.SubmissionId);
                ViewBag.PaymentTypes = new SelectList(paymentTypes, "Id", "Name", model.PaymentTypeId);

                return View(model);
            }

            model.PaymentTypeId = model.PaymentTypeId == 0 ? null : model.PaymentTypeId;

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