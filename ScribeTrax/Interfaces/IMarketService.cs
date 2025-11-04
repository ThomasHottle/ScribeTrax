using ScribeTrax.Models;

namespace ScribeTrax.Services
{
    public interface IMarketService
    {
        MarketViewModel GetMarketById(int id);
        IEnumerable<MarketViewModel> GetAllMarkets();
    }
}