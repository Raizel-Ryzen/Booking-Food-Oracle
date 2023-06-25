using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Order;

public class UpdateStatusOrderModel
{
    [Required] public string Id { get; set; }
    [Required] public int Status { get; set; }
}

public class ListDropdownStatusOrderModel
{
    public string Id { get; set; }
    public int CurrentStatus { get; set; }
    public List<DropdownDetailModel> ListStatus { get; set; }
}