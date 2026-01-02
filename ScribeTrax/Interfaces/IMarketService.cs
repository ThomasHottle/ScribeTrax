using ScribeTrax.Models;

public interface IMarketService
{
    MarketViewModel GetMarketById(int id);
    IEnumerable<MarketViewModel> GetAllMarkets();
    int CreateMarket(MarketViewModel model);
    void UpdateMarket(MarketViewModel model);
    void DeleteMarket(int id);
}