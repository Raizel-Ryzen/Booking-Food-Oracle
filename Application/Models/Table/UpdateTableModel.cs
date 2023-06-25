using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Table;

public class UpdateTableModel : AuditModel
{
    [Required] public string Title { get; set; }
    [Required] public int SlotNumber { get; set; }
}

public class UpdateTableSessionIdModel
{
    [Required] public string Id { get; set; }
    [Required] public string SessionId { get; set; }
}