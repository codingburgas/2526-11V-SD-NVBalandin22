using SubtrackProject.Models.Base;

namespace SubtrackProject.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#6c757d";

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}