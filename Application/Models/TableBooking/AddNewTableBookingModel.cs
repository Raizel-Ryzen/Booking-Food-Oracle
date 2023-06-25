using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.TableBooking;

public class AddNewTableBookingModel
{
    [Required(ErrorMessage = "Customer Name")]
    public string CustomerName { get; set; }
    public string Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    [Required] public string TableId { get; set; }
    [Required] public int IntendTime { get; set; }
    [Required] public DateTime ReceivedDate { get; set; }
    [Required] public string TimeInfo { get; set; }
    public string Code { get; set; }
    public Guid SessionId { get; set; }
}