using System.ComponentModel.DataAnnotations;
using SubtrackProject.Models.Base;

namespace SubtrackProject.Models;

public class Category : BaseEntity
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(7)]
    public string Color { get; set; } = "#6c757d";

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}