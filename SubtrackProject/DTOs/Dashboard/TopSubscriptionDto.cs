namespace SubtrackProject.DTOs.Dashboard;

public class CategorySpendingDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public decimal MonthlyCost { get; set; }
    public int SubscriptionCount { get; set; }
}