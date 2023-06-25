namespace Domain.Helpers;

public static class PagingHelper<T>
{
    public static PagingResult<T> ToPaging(List<T> list, int pageNumber, int rowOfPage)
    {
        if (pageNumber <= 0 || rowOfPage <= 0)
        {
            pageNumber = 1;
            rowOfPage = 1;
        }

        int totalItems = list.Count;
        int skip = (pageNumber - 1) * rowOfPage;
        List<T> data = list.Count > 0
            ? list
                .Skip(skip)
                .Take(rowOfPage)
                .ToList()
            : new List<T>();

        PagingResult<T> returnData = new PagingResult<T>
        {
            Data = data,
            TotalItems = list.Count,
            TotalPages = (int)Math.Ceiling((decimal)totalItems / rowOfPage)
        };

        return returnData;
    }
}

public static class ToSkipPaging
{
    public static int ToSkip(int pageNumber, int rowOfPage)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        
        if (rowOfPage <= 0)
        {
            rowOfPage = 1;
        }

        return (pageNumber - 1) * rowOfPage;
    }
}

public class PagingResult<T>
{
    public List<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}