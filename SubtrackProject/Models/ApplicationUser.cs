using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SubtrackProject.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}