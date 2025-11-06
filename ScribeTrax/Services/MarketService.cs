using ScribeTrax.Context;
using ScribeTrax.Models;

namespace ScribeTrax.Services
{
    public class MarketService : IMarketService
    {
        private readonly ScribeTraxDbContext _context;

        public MarketService(ScribeTraxDbContext context)
        {
            _context = context;
        }

        public MarketViewModel GetMarketById(int id)
        {
            var market = _context.Markets.FirstOrDefault(m => m.MarketId == id);
            if (market == null) return null;

            return new MarketViewModel
            {
                MarketId = market.MarketId,
                Name = market.Name,
                Editor = market.Editor,
                Type = market.Type,
                Email = market.Email,
                Url = market.Url,
                Postal = market.Postal
            };
        }

        public IEnumerable<MarketViewModel> GetAllMarkets()
        {
            return _context.Markets
                .OrderBy(m => m.Name)
                .Select(m => new MarketViewModel
                {
                    MarketId = m.MarketId,
                    Name = m.Name,
                    Editor = m.Editor,
                    Type = m.Type,
                    Email = m.Email,
                    Url = m.Url,
                    Postal = m.Postal
                })
                .ToList();
        }

        public void UpdateMarket(MarketViewModel model)
        {
            var entity = _context.Markets.FirstOrDefault(m => m.MarketId == model.MarketId);
            if (entity == null) return;

            entity.Name = model.Name;
            entity.Editor = model.Editor;
            entity.Type = model.Type;
            entity.Email = model.Email;
            entity.Url = model.Url;
            entity.Postal = model.Postal;

            _context.SaveChanges();
        }

        public void DeleteMarket(int id)
        {
            var entity = _context.Markets.FirstOrDefault(m => m.MarketId == id);
            if (entity == null) return;

            _context.Markets.Remove(entity);
            _context.SaveChanges();
        }
    }
}