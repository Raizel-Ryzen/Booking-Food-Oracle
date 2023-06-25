namespace Application.Models.TableBooking;

public class TableBookingFilterModel
{
    public string Id { get; set; }
    public int? Status { get; set; }
    public string Title { get; set; }
    public int PageNumber { get; set; }
    public int RowOfPage { get; set; }
}