using System.ComponentModel.DataAnnotations;

namespace Application.Models.Table;

public class UpdateStatusTableModel
{
    [Required] public string Id { get; set; }
    [Required] public int Status { get; set; }
    public string SessionId { get; set; }
}