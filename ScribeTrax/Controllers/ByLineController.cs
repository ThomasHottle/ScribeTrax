using Microsoft.AspNetCore.Mvc;
using ScribeTrax.Interfaces;
using ScribeTrax.ViewModels;

namespace ScribeTrax.Controllers
{
    public class BylineController : Controller
    {
        private readonly IBylineService _bylineService;

        public BylineController(IBylineService bylineService)
        {
            _bylineService = bylineService;
        }

        // GET: /Byline/
        public IActionResult Index(bool includeInactive = false)
        {
            var bylines = _bylineService.GetAllBylines(includeInactive);
            return View(bylines);
        }

        // GET: /Byline/Details/5
        public IActionResult Details(int id)
        {
            var byline = _bylineService.GetBylineById(id);
            if (byline == null) return NotFound();
            return View(byline);
        }

        // GET: /Byline/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Byline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BylineCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _bylineService.CreateByline(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Byline/Edit/5
        public IActionResult Edit(int id)
        {
            var byline = _bylineService.GetBylineById(id);
            if (byline == null) return NotFound();

            var updateModel = new BylineUpdateModel
            {
                Name = byline.Name,
                Type = byline.Type,
                IsInactive = byline.IsInactive
            };

            return View(updateModel);
        }

        // POST: /Byline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BylineUpdateModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _bylineService.UpdateByline(id, model);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Byline/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id)
        {
            _bylineService.DeactivateByline(id);
            return RedirectToAction(nameof(Index));
        }
    }
}