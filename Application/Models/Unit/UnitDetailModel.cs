using Application.Common.Models;

namespace Application.Models.Unit;

public class UnitDetailModel : AuditModel
{
    public string Title { get; set; }
    public string Code { get; set; }
}