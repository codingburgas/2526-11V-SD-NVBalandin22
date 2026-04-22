using Microsoft.EntityFrameworkCore;
using SubtrackProject.Data;
using SubtrackProject.DTOs.Category;
using SubtrackProject.Models;
using SubtrackProject.Services.Interfaces;

namespace SubtrackProject.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryListDto>> GetAllAsync()
    {
        // Project entities into DTOs and count related subscriptions
        return await _context.Categories
            .Select(c => new CategoryListDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Color = c.Color,
                SubscriptionCount = c.Subscriptions.Count
            })
            .ToListAsync();
    }

    public async Task<CategoryEditDto?> GetByIdAsync(int id)
    {
        var c = await _context.Categories.FindAsync(id);
        if (c == null) return null;

        return new CategoryEditDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Color = c.Color
        };
    }

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(CategoryEditDto dto)
    {
        var category = await _context.Categories.FindAsync(dto.Id);
        if (category == null) return false;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Color = dto.Color;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Subscriptions)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null) return false;
        if (category.Subscriptions.Any()) return false; // Restrict: don't delete if in use

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}