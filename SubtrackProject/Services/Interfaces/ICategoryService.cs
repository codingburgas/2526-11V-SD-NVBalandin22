using SubtrackProject.DTOs.Category;

namespace SubtrackProject.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryListDto>> GetAllAsync();
    Task<CategoryEditDto?> GetByIdAsync(int id);
    Task<string?> CreateAsync(CategoryCreateDto dto);
    Task<string?> UpdateAsync(CategoryEditDto dto);
    Task<bool> DeleteAsync(int id);
}