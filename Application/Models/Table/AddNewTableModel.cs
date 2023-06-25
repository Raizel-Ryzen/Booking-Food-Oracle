using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Table;

public class AddNewTableModel
{
    [Required] public string Title { get; set; }
    public string QRCode { get; set; }
    [Required] public int SlotNumber { get; set; }
    public string Code { get; set; }
}