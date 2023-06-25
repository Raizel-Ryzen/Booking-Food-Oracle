using System.ComponentModel.DataAnnotations;

namespace Application.Models.TableBooking;

public class UpdateStatusTableBookingModel
{
    [Required] public string Id { get; set; }
    [Required] public int Status { get; set; }
    public string SessionId { get; set; }
}