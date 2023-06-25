using Application.Common.Models;

namespace Application.Models.Categories;

public class CategoryFilterModel
{
    public string Title { get; set; }
    public int PageNumber { get; set; }
    public int RowOfPage { get; set; }
}