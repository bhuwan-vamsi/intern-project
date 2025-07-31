using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace APIPractice.Services.IService
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductAsync(string? categoryName, string? filterQuery,string? sortBy, bool IsAscending,int PageNumber,int PageSize);
        Task<ProductDto> GetProductAsync(Guid id, string role);
        Task<Product> CreateProductAsync(CreateProductDto product, Guid managerId);
        Task UpdateProductAsync(Guid id, UpdateProductDto product, Guid managerId);
        Task DeleteProductAsync(Guid id);
    }
}
