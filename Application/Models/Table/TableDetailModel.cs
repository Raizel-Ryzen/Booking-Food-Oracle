using Application.Common.Models;

namespace Application.Models.Table;

public class TableDetailModel : AuditModel
{
    public string Code { get; set; }
    public string Title { get; set; }
    public string QRCode { get; set; }
    public int SlotNumber { get; set; }
    public int Status { get; set; }
    public Guid? SessionId { get; set; }
}