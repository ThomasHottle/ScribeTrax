using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Interfaces;
using ScribeTrax.Models;
using ScribeTrax.ViewModels;
using ScribeTrax.Services;

namespace ScribeTrax.Controllers
{
    public class WorkController : Controller
    {
        private readonly IWorkService _workService;

        public WorkController(IWorkService workService)
        {
            _workService = workService;
        }

        // GET: /Work/
        public IActionResult Index()
        {
            var works = _workService.GetAllWorks();
            return View(works);
        }

        // GET: /Work/Details/5
        public IActionResult Details(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null) return NotFound();
            return View(work);
        }

        // GET: /Work/Create
        public IActionResult Create(int? bylineId)
        {
            var bylines = _workService.GetAllBylines();
            var genres = _workService.GetAllGenres();

            ViewBag.Bylines = new SelectList(bylines, "BylineId", "Name", bylineId);
            ViewBag.Genres = new SelectList(genres, "GenreId", "Name");

            var model = new WorkCreateModel
            {
                BylineId = bylineId
            };

            return View(model);
        }

        // POST: /Work/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var bylines = _workService.GetAllBylines();
                var genres = _workService.GetAllGenres();

                ViewBag.Bylines = new SelectList(bylines, "BylineId", "Name", model.BylineId);
                ViewBag.Genres = new SelectList(genres, "GenreId", "Name", model.GenreId);

                return View(model);
            }

            _workService.CreateWork(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Work/Edit/5
        public IActionResult Edit(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null) return NotFound();

            // hydrate dropdowns
            ViewBag.Bylines = new SelectList(_workService.GetAllBylines(), "BylineId", "Name", work.BylineId);
            ViewBag.Genres = new SelectList(_workService.GetAllGenres(), "GenreId", "Name", work.GenreId);

            return View(work); // pass WorkViewModel directly
        }

        // POST: /Work/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WorkUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Bylines = new SelectList(_workService.GetAllBylines(), "BylineId", "Name", model.BylineId);
                ViewBag.Genres = new SelectList(_workService.GetAllGenres(), "GenreId", "Name", model.GenreId);
                return View(model);
            }

            var entity = new Work
            {
                WorkId = model.WorkId,
                Title = model.Title,
                Type = model.Type,
                BylineId = model.BylineId,
                GenreId = model.GenreId
            };

            _workService.UpdateWork(entity);

            return RedirectToAction("Details", new { id = model.WorkId });
        }

        // GET: /Work/Delete/5
        public IActionResult Delete(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null) return NotFound();

            return View(work); // pass WorkViewModel to the view
        }

        // POST: /Work/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _workService.DeleteWork(id);
            return RedirectToAction(nameof(Index));
        }
    }
}