using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Context;
using ScribeTrax.Models;
using ScribeTrax.Services;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly ScribeTraxDbContext _context;
        private readonly ISubmissionService _submissionService;
        private static readonly List<string> SubmissionStatuses = new()
        {
            "Pending",
            "Accepted",
            "Rejected",
            "Published",
            "Open"
        };
        public IEnumerable<Work> GetWorks() => _context.Works.ToList();
        public IEnumerable<Market> GetMarkets() => _context.Markets.ToList();

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        // GET: /Submission/
        public IActionResult Index()
        {
            var submissions = _submissionService.GetAllSubmissions();
            return View(submissions);
        }

        // GET: /Submission/Details/5
        public IActionResult Details(int id)
        {
            if (id <= 0) return BadRequest();

            var submission = _submissionService.GetSubmissionById(id);
            if (submission == null) return NotFound();

            // Resolve names for display
            ViewBag.WorkTitle = _submissionService.GetWorks()
                .FirstOrDefault(w => w.WorkId == submission.WorkId)?.Title;

            ViewBag.BylineName = _submissionService.GetBylines()
                .FirstOrDefault(b => b.BylineId == submission.BylineId)?.Name;

            ViewBag.MarketName = _submissionService.GetMarkets()
                .FirstOrDefault(m => m.MarketId == submission.MarketId)?.Name;

            return View(submission);
        }

        // GET: /Submission/Create
        public IActionResult Create()
        {
            ViewBag.Works = new SelectList(_submissionService.GetWorks(), "WorkId", "Title");
            ViewBag.Markets = new SelectList(_submissionService.GetMarkets(), "MarketId", "Name");

            var model = new SubmissionViewModel
            {
                SubmissionDate = DateTime.Now,
                Status = "Pending"
            };

            return View(model);
        }

        // POST: /Submission/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubmissionViewModel model)
        {
            if (model == null || !ModelState.IsValid)
                return View(model);

            _submissionService.CreateSubmission(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Submission/Edit/5
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var submissionVm = _submissionService.GetSubmissionById(id);
            if (submissionVm == null) return NotFound();

            ViewBag.Works = new SelectList(_submissionService.GetWorks(), "WorkId", "Title", submissionVm.WorkId);
            ViewBag.Markets = new SelectList(_submissionService.GetMarkets(), "MarketId", "Name", submissionVm.MarketId);
            ViewBag.Statuses = new SelectList(SubmissionStatuses, submissionVm.Status);

            return View(submissionVm);
        }



        // POST: /Submission/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubmissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Works = new SelectList(_submissionService.GetWorks(), "WorkId", "Title", model.WorkId);
                ViewBag.Markets = new SelectList(_submissionService.GetMarkets(), "MarketId", "Name", model.MarketId);
                ViewBag.Statuses = new SelectList(SubmissionStatuses, model.Status);

                return View(model);
            }

            _submissionService.UpdateSubmission(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Submission/Delete/5
        public IActionResult Delete(int id)
        {
            var submission = _submissionService.GetSubmissionById(id);
            if (submission == null) return NotFound();

            // Populate names for display
            ViewBag.WorkTitle = _submissionService.GetWorks()
                .FirstOrDefault(w => w.WorkId == submission.WorkId)?.Title;

            ViewBag.BylineName = _submissionService.GetBylines()
                .FirstOrDefault(b => b.BylineId == submission.BylineId)?.Name;

            ViewBag.MarketName = _submissionService.GetMarkets()
                .FirstOrDefault(m => m.MarketId == submission.MarketId)?.Name;

            return View(submission);
        }

        // POST: /Submission/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _submissionService.DeleteSubmission(id);
            return RedirectToAction(nameof(Index));
        }
    }
}