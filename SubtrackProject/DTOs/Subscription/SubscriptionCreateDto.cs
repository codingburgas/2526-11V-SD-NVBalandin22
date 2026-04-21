using System.ComponentModel.DataAnnotations;
using SubtrackProject.Models;

namespace SubtrackProject.DTOs.Subscription;

public class SubscriptionCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Required]
    public BillingCycle BillingCycle { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    public DateTime NextPaymentDate { get; set; } = DateTime.Today.AddMonths(1);

    public bool IsActive { get; set; } = true;

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}