namespace Application.PagedList;

public class PaginationModel<T>
{
    /// <summary>
    /// Page Index
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Page Size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total Count
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total Pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Items
    /// </summary>
    public IEnumerable<T> Items { get; set; }
}
