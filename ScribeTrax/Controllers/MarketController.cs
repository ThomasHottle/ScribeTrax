using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScribeTrax.Models;
using ScribeTrax.Services;

namespace ScribeTrax.Controllers
{
    public class MarketController : Controller
    {
        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        // GET: /Market/
        public IActionResult Index()
        {
            var markets = _marketService.GetAllMarkets();
            return View(markets);
        }

        // GET: /Market/Details/5
        public IActionResult Details(int id)
        {
            var market = _marketService.GetMarketById(id);
            if (market == null)
            {
                return NotFound();
            }
            return View(market);
        }

        // GET: /Market/Create
        public IActionResult Create(int? workId)
        {
            var model = new MarketViewModel
            {
                // Preserve the WorkId so we can return to Submission/Create
                WorkId = workId
            };

            return View(model);
        }

        // POST: /Market/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MarketViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newMarketId = _marketService.CreateMarket(model);

            return RedirectToAction(
                "Create",
                "Submission",
                new { workId = model.WorkId, marketId = newMarketId }
            );
        }


        // GET: /Market/Edit/5
        public IActionResult Edit(int id)
        {
            var market = _marketService.GetMarketById(id);
            if (market == null)
            {
                return NotFound();
            }
            return View(market);
        }

        // POST: /Market/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MarketViewModel model)
        {
            if (id != model.MarketId) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _marketService.UpdateMarket(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Market/Delete/5
        public IActionResult Delete(int id)
        {
            var market = _marketService.GetMarketById(id);
            if (market == null)
            {
                return NotFound();
            }
            return View(market);
        }

        // POST: /Market/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _marketService.DeleteMarket(id);
            return RedirectToAction(nameof(Index));
        }
    }
}