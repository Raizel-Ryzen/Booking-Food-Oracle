using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Table : Audit
{
    [Required] public string Code { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string QRCode { get; set; }
    [Required] public int SlotNumber { get; set; }
    [Required] public int Status { get; set; }
    public Guid? SessionId { get; set; }
}