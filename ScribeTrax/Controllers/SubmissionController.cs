using Microsoft.AspNetCore.Mvc;
using ScribeTrax.Models;
using ScribeTrax.Services;

namespace ScribeTrax.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly ISubmissionService _submissionService;

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
            var submission = _submissionService.GetSubmissionById(id);
            if (submission == null)
            {
                return NotFound();
            }
            return View(submission);
        }

        // GET: /Submission/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Submission/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Submission submission)
        {
            if (!ModelState.IsValid)
            {
                return View(submission);
            }

            _submissionService.CreateSubmission(submission);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Submission/Edit/5
        public IActionResult Edit(int id)
        {
            var submission = _submissionService.GetSubmissionById(id);
            if (submission == null)
            {
                return NotFound();
            }

            // You may want to map ViewModel back to Submission for editing
            var model = new Submission
            {
                SubmissionId = submission.SubmissionId,
                WorkId = submission.WorkId,
                MarketId = submission.MarketId,
                SubmissionDate = submission.SubmissionDate
                // Add other fields as needed
            };

            return View(model);
        }

        // POST: /Submission/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Submission submission)
        {
            if (id != submission.SubmissionId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(submission);
            }

            _submissionService.UpdateSubmission(submission);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Submission/Delete/5
        public IActionResult Delete(int id)
        {
            var submission = _submissionService.GetSubmissionById(id);
            if (submission == null)
            {
                return NotFound();
            }
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