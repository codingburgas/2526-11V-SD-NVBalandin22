using System.ComponentModel.DataAnnotations;
using SubtrackProject.Models;

namespace SubtrackProject.DTOs.Subscription;

public class SubscriptionEditDto
{
    [Required]
    public int Id { get; set; }

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
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NextPaymentDate { get; set; }

    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}