using Application.Common.Models;

namespace Application.Models.Categories;

public class CategoryDetailModel : AuditModel
{
    public string Title { get; set; }
    public string Code { get; set; }
}