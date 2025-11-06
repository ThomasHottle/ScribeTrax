using Microsoft.AspNetCore.Mvc;
using ScribeTrax.Interfaces;
using ScribeTrax.Models;
using ScribeTrax.ViewModels;

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
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Work/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = new Work
            {
                Title = model.Title,
                Type = model.Type,
                BylineId = model.BylineId,
                GenreId = model.GenreId
            };

            _workService.CreateWork(entity);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Work/Edit/5
        public IActionResult Edit(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null) return NotFound();

            var updateModel = new WorkUpdateModel
            {
                Title = work.Title,
                Type = work.Type,
                BylineId = work.BylineId,
                GenreId = work.GenreId
            };

            return View(updateModel);
        }

        // POST: /Work/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, WorkUpdateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = new Work
            {
                WorkId = id,
                Title = model.Title,
                Type = model.Type,
                BylineId = (int)model.BylineId,
                GenreId = model.GenreId
            };

            _workService.UpdateWork(entity);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Work/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _workService.DeleteWork(id);
            return RedirectToAction(nameof(Index));
        }
    }
}