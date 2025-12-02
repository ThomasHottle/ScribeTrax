using ScribeTrax.Models;

public interface IMarketService
{
    MarketViewModel GetMarketById(int id);
    IEnumerable<MarketViewModel> GetAllMarkets();
    void CreateMarket(MarketViewModel model);   // ✅ new
    void UpdateMarket(MarketViewModel model);
    void DeleteMarket(int id);
}