using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScribeTrax.Interfaces;
using ScribeTrax.ViewModels;
using ScribeTrax.Services;

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

        // GET: Byline/Create
        public IActionResult Create()
        {
            var model = new BylineCreateModel
            {
                TypeOptions = GetTypeOptions()
            };
            return View(model);
        }

        // POST: Byline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BylineCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TypeOptions = GetTypeOptions(); // Rehydrate dropdown
                return View(model);
            }

            // ✅ Service should return the new ID
            int newId = _bylineService.CreateByline(model);

            // Redirect to Details so you can immediately see the new record
            return RedirectToAction("Details", new { id = newId });
        }


        // GET: Byline/Edit/5
        public IActionResult Edit(int id)
        {
            var entity = _bylineService.GetBylineById(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new BylineUpdateModel
            {
                BylineId = entity.BylineId,
                Name = entity.Name,
                Type = entity.Type,
                IsInactive = entity.IsInactive,
                TypeOptions = GetTypeOptions() // ✅ hydrate dropdown
            };

            return View(model);
        }

        // POST: Byline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BylineUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TypeOptions = GetTypeOptions(); // ✅ rehydrate if validation fails
                return View(model);
            }

            _bylineService.UpdateByline(model);
            return RedirectToAction("Details", new { id = model.BylineId });
        }

        // Helper method
        private List<SelectListItem> GetTypeOptions() => new()
        {
            new SelectListItem { Value = "Author", Text = "Author" },
            new SelectListItem { Value = "Co-Author", Text = "Co-Author" },
            new SelectListItem { Value = "Ghost", Text = "Ghost" }
        };

        // GET: Byline/Delete/5
        public IActionResult Delete(int id)
        {
            var entity = _bylineService.GetBylineById(id);
            if (entity == null)
            {
                return NotFound();
            }

            // You can pass a simple view model here for confirmation
            var model = new BylineUpdateModel
            {
                BylineId = entity.BylineId,
                Name = entity.Name,
                Type = entity.Type,
                IsInactive = entity.IsInactive
            };

            return View(model);
        }

        // POST: Byline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bylineService.DeleteByline(id);
            return RedirectToAction(nameof(Index));
        }

    }
}