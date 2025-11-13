using ScribeTrax.Models;

public static class BylineQueries
{
    public static IQueryable<Byline> ActiveOnly(this IQueryable<Byline> query) =>
        query.Where(b => b.Inactive == false || b.Inactive == null);
}