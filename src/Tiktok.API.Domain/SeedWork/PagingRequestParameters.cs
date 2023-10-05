namespace Tiktok.API.Domain.SeedWork;

public class PagingRequestParameters
{
    protected const int MaxPageSize = 50;
    
    protected int _pageNumber = 1;

    protected int _pageSize = 10;
    
    /// <summary>
    /// 1 = asc, 2 = desc
    /// </summary>
    protected byte _orderBy = 1;

    public virtual string? OrderBy
    {
        get => _orderBy == 1 ? "asc" : "desc";
        set => _orderBy = value == "asc" ? (byte) 1 : (byte) 2;
    }
    
    public virtual int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public virtual int PageSize
    {
        get => _pageSize;
        set
        {
            if (value > 0)
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
