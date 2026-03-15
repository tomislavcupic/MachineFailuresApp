using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineFailures.Application;

public class CategoryService
{
    private readonly MachineFailureDbContext _context;

    public CategoryService(MachineFailureDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> ListCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("category name is required.", nameof(category.Name));

        var exists = await _context.Categories.AnyAsync(c => c.Name == category.Name);
        if (exists)
        {
            throw new InvalidOperationException("There is already a category with that name");
        }

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }
}