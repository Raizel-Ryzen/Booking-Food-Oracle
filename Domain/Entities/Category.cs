using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category : Audit
{
    [Required] public string Title { get; set; }
    [Required] public string Code { get; set; }
}