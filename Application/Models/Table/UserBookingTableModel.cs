using System.ComponentModel.DataAnnotations;

namespace Application.Models.Table;

public class UserBookingTableModel
{
    public string SessionId { get; set; }
    public string TableId { get; set; }
    public string TableName { get; set; }
    public string QRCode { get; set; }
}
