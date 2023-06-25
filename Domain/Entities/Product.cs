using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product : Audit
{
    [Required] public string Title { get; set; }
    [Required] public string Code { get; set; }
    [Required] public string Thumbnail { get; set; }
    [Required] public double Amount { get; set; }
    [Required] public int Status { get; set; }
    [Required] public int Bought { get; set; }
    [Required] public string Url { get; set; }
    [Required] public Guid CategoryId { get; set; }
    [Required] public Guid UnitId { get; set; }
}