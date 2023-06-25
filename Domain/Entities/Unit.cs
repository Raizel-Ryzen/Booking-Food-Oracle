using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Unit : Audit
{
    [Required] public string Title { get; set; }
    [Required] public string Code { get; set; }
}