using System.ComponentModel.DataAnnotations;

namespace SubtrackProject.DTOs.Category;

public class CategoryCreateDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    [Required]
    [StringLength(7)]
    [Display(Name = "Color (hex)")]
    public string Color { get; set; } = "#6c757d";
}