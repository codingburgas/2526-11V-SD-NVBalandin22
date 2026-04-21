using Microsoft.AspNetCore.Identity;

namespace SubtrackProject.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}