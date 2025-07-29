using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIPractice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository<Product> productRepo;
        private readonly IMapper mapper;

        public ProductService(IProductRepository<Product> productRepo, IMapper mapper) 
        {
            this.productRepo = productRepo;
            this.mapper = mapper;
        }
        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = mapper.Map<Product>(createProductDto);
            return (await productRepo.CreateAsync(product));
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await productRepo.GetAsync(id);
            if (product == null) 
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            await productRepo.DeleteAsync(product);
        }

        public async Task<List<ProductDto>> GetAllProductAsync(string? categoryName, string? filterQuery)
        {
            var products = await productRepo.GetAllAsync(categoryName, filterQuery);
            return mapper.Map<List<ProductDto>>(products);
        }
        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            var product = await productRepo.GetAsync(id);
            return mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDto product, Guid managerId)
        {
            var existingProduct = await productRepo.GetAsync(id);
            if (existingProduct == null) 
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            await productRepo.UpdateAsync(existingProduct,product, managerId);
        }
    }
}
