using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TableBooking : Audit
{
    [Required] public string Code { get; set; }
    [Required] public string CustomerName { get; set; }
    public string? Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    [Required] public Guid TableId { get; set; }
    [Required] public int Status { get; set; }
    [Required] public int IntendTime { get; set; }
    [Required] public DateTime ReceivedDate { get; set; }
    [Required] public string TimeInfo { get; set; }
    public Guid? SessionId { get; set; }
    [Required] public string RestaurantAddress { get; set; }
    [Required] public bool IsEdit { get; set; }
}