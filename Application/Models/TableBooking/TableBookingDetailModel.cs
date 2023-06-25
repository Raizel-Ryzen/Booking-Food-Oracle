using Application.Common.Models;

namespace Application.Models.TableBooking;

public class TableBookingDetailModel : AuditModel
{
    public string Code { get; set; }
    public string CustomerName { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; }
    public Guid TableId { get; set; }
    public int Status { get; set; }
    public int IntendTime { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string TimeInfo { get; set; }
    public Guid? SessionId { get; set; }
    public string TableName { get; set; }
    public int TableStatus { get; set; }
    public string RestaurantAddress { get; set; }
    public string QRCode { get; set; }
    public bool IsEdit { get; set; }
}