using SubtrackProject.DTOs.Dashboard;
using SubtrackProject.DTOs.Subscription;

namespace SubtrackProject.Services.Interfaces;

public interface ISubscriptionService
{
    // CRUD — scoped per user
    Task<List<SubscriptionListDto>> GetAllForUserAsync(string userId);
    Task<SubscriptionEditDto?> GetByIdAsync(int id, string userId);
    Task CreateAsync(SubscriptionCreateDto dto, string userId);
    Task<bool> UpdateAsync(SubscriptionEditDto dto, string userId);
    Task<bool> DeleteAsync(int id, string userId);

    // Calculations
    Task<DashboardStatsDto> GetDashboardStatsAsync(string userId);
    Task<decimal> GetMonthlyTotalAsync(string userId);
    Task<decimal> GetYearlyTotalAsync(string userId);
    Task<List<TopSubscriptionDto>> GetTopThreeAsync(string userId);
    Task<List<CategorySpendingDto>> GetSpendingByCategoryAsync(string userId);
}