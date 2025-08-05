using APIPractice.Models.Domain;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync();
}