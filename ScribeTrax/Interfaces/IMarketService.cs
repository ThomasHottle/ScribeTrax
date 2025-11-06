using ScribeTrax.Models;

namespace ScribeTrax.Services
{
    public interface IMarketService
    {
        MarketViewModel GetMarketById(int id);
        IEnumerable<MarketViewModel> GetAllMarkets();
        void UpdateMarket(MarketViewModel model);
        void DeleteMarket(int id);
    }
}