namespace SubtrackProject.DTOs.Category;

public class CategoryListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = string.Empty;
    public int SubscriptionCount { get; set; }
}