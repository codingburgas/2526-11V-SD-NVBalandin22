using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubtrackProject.DTOs.Category;
using SubtrackProject.Services.Interfaces;

namespace SubtrackProject.Controllers;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: /Category
    // List of all categories
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }

    // GET: /Category/Create
    // Empty form for creating a new category
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Category/Create
    // Handle the submitted form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var error = await _categoryService.CreateAsync(dto);
        if (error != null)
        {
            ModelState.AddModelError(nameof(dto.Name), error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: /Category/Edit/5
    // Form pre-filled with category data
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _categoryService.GetByIdAsync(id);
        if (dto == null) return NotFound();

        return View(dto);
    }

    // POST: /Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryEditDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var error = await _categoryService.UpdateAsync(dto);
        if (error != null)
        {
            ModelState.AddModelError(nameof(dto.Name), error);
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted)
            TempData["Error"] = "Cannot delete a category that has subscriptions.";

        return RedirectToAction(nameof(Index));
    }
}