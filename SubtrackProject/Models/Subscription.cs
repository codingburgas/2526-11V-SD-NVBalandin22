using SubtrackProject.Models.Base;

namespace SubtrackProject.Models;

public class Subscription : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public Category Category { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}