namespace SubtrackProject.DTOs.Dashboard;

public class DashboardStatsDto
{
    public decimal TotalMonthlyCost { get; set; }
    public decimal TotalYearlyCost { get; set; }
    public int ActiveSubscriptionsCount { get; set; }

    public List<TopSubscriptionDto> TopThreeMostExpensive { get; set; } = new();
    public List<CategorySpendingDto> SpendingByCategory { get; set; } = new();
}