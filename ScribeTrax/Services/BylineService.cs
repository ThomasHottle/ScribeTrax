namespace ScribeTrax.Services
{
    using Microsoft.EntityFrameworkCore;
    using ScribeTrax.Context;
    using ScribeTrax.Interfaces;
    using ScribeTrax.Models; // Your ViewModels and Create/Update models
    using ScribeTrax.ViewModels;

    public class BylineService : IBylineService
    {
        private readonly ScribeTraxDbContext _context;

        public BylineService(ScribeTraxDbContext context)
        {
            _context = context;
        }

// Fix for CS0266 and CS8629: Explicitly handle nullable bool conversion and possible null value
public IEnumerable<BylineViewModel> GetAllBylines(bool includeInactive = false)
{
        var query = includeInactive ? _context.Bylines : _context.Bylines.ActiveOnly();

            if (!includeInactive)
        query = query.Where(b => b.Inactive == false || b.Inactive == null);

    return query
        .OrderBy(b => b.Name)
        .Select(b => new BylineViewModel
        {
            BylineId = b.BylineId,
            Name = b.Name,
            Type = b.Type,
            IsInactive = b.Inactive ?? false // Explicitly handle null by providing a default value
        })
        .ToList();
}

public BylineViewModel GetBylineById(int id)
{
    var b = _context.Bylines.Find(id);
    if (b == null) return null;

    return new BylineViewModel
    {
        BylineId = b.BylineId,
        Name = b.Name,
        Type = b.Type,
        IsInactive = b.Inactive ?? false // Explicitly handle null by providing a default value
    };
}

        public int CreateByline(BylineCreateModel model)
        {
            var entity = new Byline
            {
                Name = model.Name,
                Type = model.Type,
                Inactive = model.IsInactive
            };

            _context.Bylines.Add(entity);

            // ✅ Without this, nothing is written to SQL
            _context.SaveChanges();

            return entity.BylineId;
        }



        public void UpdateByline(BylineUpdateModel model)
        {
            int id = model.BylineId;
            var entity = _context.Bylines.Find(id);
            if (entity == null) return;

            if (!string.IsNullOrWhiteSpace(model.Name))
                entity.Name = model.Name;

            if (!string.IsNullOrWhiteSpace(model.Type))
                entity.Type = model.Type;

            if (model.IsInactive.HasValue)
                entity.Inactive = model.IsInactive.Value;

            _context.SaveChanges();
        }

        public void DeleteByline(int id)
        {
            var entity = _context.Bylines.Find(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Byline with ID {id} not found.");
            }

            _context.Bylines.Remove(entity);
            _context.SaveChanges();
        }
    }
}
