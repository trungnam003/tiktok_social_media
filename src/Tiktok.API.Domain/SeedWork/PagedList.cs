using Microsoft.EntityFrameworkCore;

namespace Tiktok.API.Domain.SeedWork;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalItems = totalItems,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
        AddRange(items);
    }
    private MetaData MetaData { get; }

    public MetaData GetMetaData()
    {
        return MetaData;
    }
    
    public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize
    )
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}