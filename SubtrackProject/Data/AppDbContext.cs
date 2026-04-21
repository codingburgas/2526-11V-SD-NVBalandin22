using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SubtrackProject.Models;

namespace SubtrackProject.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // ALWAYS call base first when using IdentityDbContext
        base.OnModelCreating(builder);

        // User → Subscriptions (one-to-many)
        builder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade); // If user is deleted → their subscriptions are deleted too

        // Category → Subscriptions (one-to-many)
        builder.Entity<Subscription>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete a category that still has subscriptions
    }
}