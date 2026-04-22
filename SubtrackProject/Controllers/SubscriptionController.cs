using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubtrackProject.DTOs.Subscription;
using SubtrackProject.Services.Interfaces;

namespace SubtrackProject.Controllers;

[Authorize]
public class SubscriptionController : Controller
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ICategoryService _categoryService;

    public SubscriptionController(
        ISubscriptionService subscriptionService,
        ICategoryService categoryService)
    {
        _subscriptionService = subscriptionService;
        _categoryService = categoryService;
    }

    // Helper: get current user's ID from the login cookie
    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: /Subscription
    // List of the current user's subscriptions
    public async Task<IActionResult> Index()
    {
        var list = await _subscriptionService.GetAllForUserAsync(CurrentUserId);
        return View(list);
    }

    // GET: /Subscription/Create
    // Empty form with categories dropdown
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View();
    }

    // POST: /Subscription/Create
    [HttpPost]
    public async Task<IActionResult> Create(SubscriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync();
            return View(dto);
        }

        await _subscriptionService.CreateAsync(dto, CurrentUserId);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Subscription/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _subscriptionService.GetByIdAsync(id, CurrentUserId);
        if (dto == null) return NotFound();

        await PopulateCategoriesAsync();
        return View(dto);
    }

    // POST: /Subscription/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(SubscriptionEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync();
            return View(dto);
        }

        var updated = await _subscriptionService.UpdateAsync(dto, CurrentUserId);
        if (!updated) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // POST: /Subscription/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _subscriptionService.DeleteAsync(id, CurrentUserId);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Subscription/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var list = await _subscriptionService.GetAllForUserAsync(CurrentUserId);
        var sub = list.FirstOrDefault(s => s.Id == id);
        if (sub == null) return NotFound();

        return View(sub);
    }
    
    
    
    // Fills the categories dropdown for Create/Edit forms
    private async Task PopulateCategoriesAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
    }
}