using Microsoft.EntityFrameworkCore;
using SubtrackProject.Data;
using SubtrackProject.DTOs.Dashboard;
using SubtrackProject.DTOs.Subscription;
using SubtrackProject.Models;
using SubtrackProject.Services.Interfaces;

namespace SubtrackProject.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly AppDbContext _context;

    public SubscriptionService(AppDbContext context)
    {
        _context = context;
    }

    // ---------- CRUD ----------

    public async Task<List<SubscriptionListDto>> GetAllForUserAsync(string userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.IsActive)
            .ThenBy(s => s.Name)
            .Select(s => new SubscriptionListDto
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                BillingCycle = s.BillingCycle,
                NextPaymentDate = s.NextPaymentDate,
                IsActive = s.IsActive,
                CategoryName = s.Category.Name,
                CategoryColor = s.Category.Color
            })
            .ToListAsync();
    }

    public async Task<SubscriptionEditDto?> GetByIdAsync(int id, string userId)
    {
        // Security: only return the record if it belongs to the current user
        var s = await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (s == null) return null;

        return new SubscriptionEditDto
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price,
            BillingCycle = s.BillingCycle,
            StartDate = s.StartDate,
            NextPaymentDate = s.NextPaymentDate,
            IsActive = s.IsActive,
            CategoryId = s.CategoryId
        };
    }

    public async Task CreateAsync(SubscriptionCreateDto dto, string userId)
    {
        var subscription = new Subscription
        {
            Name = dto.Name,
            Price = dto.Price,
            BillingCycle = dto.BillingCycle,
            StartDate = dto.StartDate,
            NextPaymentDate = dto.NextPaymentDate,
            IsActive = dto.IsActive,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(SubscriptionEditDto dto, string userId)
    {
        var s = await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == dto.Id && x.UserId == userId);

        if (s == null) return false;

        s.Name = dto.Name;
        s.Price = dto.Price;
        s.BillingCycle = dto.BillingCycle;
        s.StartDate = dto.StartDate;
        s.NextPaymentDate = dto.NextPaymentDate;
        s.IsActive = dto.IsActive;
        s.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var s = await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (s == null) return false;

        _context.Subscriptions.Remove(s);
        await _context.SaveChangesAsync();
        return true;
    }

    // ---------- Calculations ----------

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(string userId)
    {
        return new DashboardStatsDto
        {
            TotalMonthlyCost = await GetMonthlyTotalAsync(userId),
            TotalYearlyCost = await GetYearlyTotalAsync(userId),
            ActiveSubscriptionsCount = await _context.Subscriptions
                .CountAsync(s => s.UserId == userId && s.IsActive),
            TopThreeMostExpensive = await GetTopThreeAsync(userId),
            SpendingByCategory = await GetSpendingByCategoryAsync(userId)
        };
    }

    public async Task<decimal> GetMonthlyTotalAsync(string userId)
    {
        // Normalize: monthly price = yearly price / 12
        return await _context.Subscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .SumAsync(s => s.BillingCycle == BillingCycle.Monthly
                ? s.Price
                : s.Price / 12m);
    }

    public async Task<decimal> GetYearlyTotalAsync(string userId)
    {
        // Normalize: yearly price = monthly price * 12
        return await _context.Subscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .SumAsync(s => s.BillingCycle == BillingCycle.Yearly
                ? s.Price
                : s.Price * 12m);
    }

    public async Task<List<TopSubscriptionDto>> GetTopThreeAsync(string userId)
    {
        // Compare everything on a monthly basis for fair ranking
        return await _context.Subscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .Select(s => new TopSubscriptionDto
            {
                Name = s.Name,
                CategoryName = s.Category.Name,
                MonthlyCost = s.BillingCycle == BillingCycle.Monthly
                    ? s.Price
                    : s.Price / 12m
            })
            .OrderByDescending(s => s.MonthlyCost)
            .Take(3)
            .ToListAsync();
    }

    public async Task<List<CategorySpendingDto>> GetSpendingByCategoryAsync(string userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .GroupBy(s => new { s.Category.Name, s.Category.Color })
            .Select(g => new CategorySpendingDto
            {
                CategoryName = g.Key.Name,
                CategoryColor = g.Key.Color,
                SubscriptionCount = g.Count(),
                MonthlyCost = g.Sum(s => s.BillingCycle == BillingCycle.Monthly
                    ? s.Price
                    : s.Price / 12m)
            })
            .OrderByDescending(x => x.MonthlyCost)
            .ToListAsync();
    }
}