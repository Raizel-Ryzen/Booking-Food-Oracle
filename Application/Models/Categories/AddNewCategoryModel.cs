using System.ComponentModel.DataAnnotations;
using Application.Common.Models;

namespace Application.Models.Categories;

public class AddNewCategoryModel
{
    [Required] public string Title { get; set; }
}