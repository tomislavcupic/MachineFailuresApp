using System.Collections;
using MachineFailures.Application;
using MachineFailures.Domain;
using MachineFailures.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MachineFailures.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _categoryService.ListCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryAsync(id);
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> AddCategory(Category category)
    {
        var newCategory = await _categoryService.AddCategoryAsync(category);
        return Ok(newCategory);
    }
}