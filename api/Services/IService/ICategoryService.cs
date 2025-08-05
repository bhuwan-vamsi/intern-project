using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategories();
    }
}
