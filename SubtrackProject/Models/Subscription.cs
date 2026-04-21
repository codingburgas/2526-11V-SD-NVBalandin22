using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SubtrackProject.Models.Base;

namespace SubtrackProject.Models;

public class Subscription : BaseEntity
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 100000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public BillingCycle BillingCycle { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime NextPaymentDate { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    public Category Category { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}