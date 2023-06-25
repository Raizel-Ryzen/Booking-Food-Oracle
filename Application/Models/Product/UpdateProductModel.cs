using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Product;

public class UpdateProductModel
{
    [Required] public string Id { get; set; }
    [Required] public string Title { get; set; }
    public string Thumbnail { get; set; }
    [Required] public double Amount { get; set; }
    [Required] public string CategoryId { get; set; }
    [Required] public string UnitId { get; set; }
    public string CurrentThumbnail { get; set; }
    public IFormFile? Image { get; set; }
}

public class UpdateStatusProductModel
{
    [Required] public string Id { get; set; }
    [Required] public int Status { get; set; }
}