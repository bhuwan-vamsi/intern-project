using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using AutoMapper;

namespace APIPractice.Services
{
    public class CategoryService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        ICategoryRepository categoryRepository;

        public CategoryService(ICustomerRepository customerRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDto>> GetCategories()
        {
            List<Category> categories = await categoryRepository.GetCategoriesAsync();
            return mapper.Map<List<CategoryDto>>(categories);
        }
    }
}
