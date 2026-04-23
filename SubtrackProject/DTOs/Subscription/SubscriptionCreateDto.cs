using System.ComponentModel.DataAnnotations;
using SubtrackProject.Models;

namespace SubtrackProject.DTOs.Subscription;

public class SubscriptionCreateDto : IValidatableObject
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100,000.")]
    public decimal Price { get; set; }

    [Required]
    public BillingCycle BillingCycle { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Start date")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Next payment date")]
    public DateTime NextPaymentDate { get; set; } = DateTime.Today.AddMonths(1);

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    // Cross-field validation: runs on the server after simple validations pass
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (NextPaymentDate.Date < DateTime.Today)
        {
            yield return new ValidationResult(
                "Next payment date cannot be in the past.",
                new[] { nameof(NextPaymentDate) });
        }

        if (NextPaymentDate.Date < StartDate.Date)
        {
            yield return new ValidationResult(
                "Next payment date must be after the start date.",
                new[] { nameof(NextPaymentDate) });
        }
    }
}