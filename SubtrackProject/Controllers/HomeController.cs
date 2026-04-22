using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtrackProject.Services.Interfaces;

namespace SubtrackProject.Controllers;

public class HomeController : Controller
{
    private readonly ISubscriptionService _subscriptionService;

    public HomeController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    // GET: /  or  /Home/Index
    // If not logged in → welcome page. If logged in → dashboard with stats.
    public async Task<IActionResult> Index()
    {
        if (!User.Identity!.IsAuthenticated)
            return View("Welcome");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var stats = await _subscriptionService.GetDashboardStatsAsync(userId);

        return View("Dashboard", stats);
    }
}