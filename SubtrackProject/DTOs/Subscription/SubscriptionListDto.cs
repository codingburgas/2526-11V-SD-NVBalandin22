using SubtrackProject.Models;

namespace SubtrackProject.DTOs.Subscription;

public class SubscriptionListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public bool IsActive { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
}