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
        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto, Guid managerId)
        {
            var product = mapper.Map<Product>(createProductDto);
            return (await productRepo.CreateAsync(product,managerId));
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

        public async Task<List<ProductDto>> GetAllProductAsync(string? categoryName, string? filterQuery, string? sortBy, bool IsAscending, int PageNumber, int PageSize)
        {
            var products = await productRepo.GetAllAsync(categoryName, filterQuery, sortBy, IsAscending, PageNumber, PageSize);
            var productDto = mapper.Map<List<ProductDto>>(products);
            foreach(var product in productDto)
            {
                if(product.Quantity < product.Threshold)
                {
                    product.ProductStatus = "LowStock";
                }
                else if (product.Quantity == 0)
                {
                    product.ProductStatus = "OutOfStock";
                }
                else
                {
                    product.ProductStatus = "InStock";
                }
            }
            return productDto;

        }
        public async Task<ProductDto> GetProductAsync(Guid id, string role)
        {
            var product = await productRepo.GetAsync(id);
            if(role == "Customer" && product.IsActive == false)
            {
                throw new KeyNotFoundException("Product Not Found");            }
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
