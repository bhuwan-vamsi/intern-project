using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Services.IService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductAsync(string? categoryName, string? filterQuery);
        Task<ProductDto> GetProductAsync(Guid id);
        Task<Product> CreateProductAsync(CreateProductDto product);
        Task UpdateProductAsync(Guid id, UpdateProductDto product, Guid managerId);
        Task DeleteProductAsync(Guid id);
    }
}
