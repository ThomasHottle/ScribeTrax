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
    }
}