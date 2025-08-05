using APIPractice.Data;
using APIPractice.Models.Domain;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext db;

    public CategoryRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await db.Categories.ToListAsync();
        return categories;
    }
}