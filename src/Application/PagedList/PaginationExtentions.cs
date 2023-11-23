using Microsoft.EntityFrameworkCore;

namespace Application.PagedList;

public static class PaginationExtensions
{
    public static PaginationModel<T> ToPagedList<T>(this IList<T> source, int? pageIndex, int? pageSize)
    {
        var totalCount = source.Count;
        if (!pageIndex.HasValue || !pageSize.HasValue)
        {
            var result = new PagedList<T>(source, 0, totalCount <= 0 ? 1 : totalCount, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = totalCount == 0 ? 0 : result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
        else
        {
            var items = source.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value).ToList();
            var result = new PagedList<T>(items, pageIndex.Value, pageSize.Value, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
    }

    public static PaginationModel<T> ToPagedList<T>(this IQueryable<T> source, int? pageIndex, int? pageSize)
    {
        var totalCount = source.Count();
        if (!pageIndex.HasValue || !pageSize.HasValue)
        {
            var result = new PagedList<T>(source.ToList(), 0, totalCount <= 0 ? 1 : totalCount, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = totalCount == 0 ? 0 : result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
        else
        {
            var items = source.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value).ToList();
            var result = new PagedList<T>(items, pageIndex.Value, pageSize.Value, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
    }

    public static async Task<PaginationModel<T>> ToPagedListAsync<T>(this IQueryable<T> source, int? pageIndex, int? pageSize)
    {
        var totalCount = await source.CountAsync();
        if (!pageIndex.HasValue || !pageSize.HasValue)
        {
            var result = new PagedList<T>(await source.ToListAsync(), 0, totalCount <= 0 ? 1 : totalCount, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = totalCount == 0 ? 0 : result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
        else
        {
            var items = await source.Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value).ToListAsync();
            var result = new PagedList<T>(items, pageIndex.Value, pageSize.Value, totalCount);
            return new PaginationModel<T>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result
            };
        }
    }
}
