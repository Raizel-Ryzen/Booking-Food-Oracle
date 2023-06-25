using Application.Common.Models;

namespace Application.Models.Table;

public class TableFilterModel
{
    public string Id { get; set; }
    public int? Status { get; set; }
    public string Title { get; set; }
    public int PageNumber { get; set; }
    public int RowOfPage { get; set; }
}