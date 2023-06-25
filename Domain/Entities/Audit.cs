using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Audit
{
    [Required] [Key] public Guid Id { get; set; }
    [Required] public bool IsDeleted { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public string CreatedBy { get; set; }
}