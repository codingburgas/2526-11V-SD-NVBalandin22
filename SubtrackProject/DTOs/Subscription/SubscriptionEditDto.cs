using System.ComponentModel.DataAnnotations;
using SubtrackProject.Models;

namespace SubtrackProject.DTOs.Subscription;

public class SubscriptionEditDto : IValidatableObject
{
    [Required]
    public int Id { get; set; }

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
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Next payment date")]
    public DateTime NextPaymentDate { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (NextPaymentDate.Date < StartDate.Date)
        {
            yield return new ValidationResult(
                "Next payment date must be after the start date.",
                new[] { nameof(NextPaymentDate) });
        }
    }
}