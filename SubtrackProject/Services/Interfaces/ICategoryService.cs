using SubtrackProject.DTOs.Category;

namespace SubtrackProject.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryListDto>> GetAllAsync();
    Task<CategoryEditDto?> GetByIdAsync(int id);
    Task CreateAsync(CategoryCreateDto dto);
    Task<bool> UpdateAsync(CategoryEditDto dto);
    Task<bool> DeleteAsync(int id);
}