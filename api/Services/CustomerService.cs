using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using System.Security.Claims;

namespace APIPractice.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public CustomerService(ICustomerRepository customerRepository,IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task EditProfile(ClaimsIdentity identity, EditProfileRequest request)
        {
            var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId == null)
            {
                throw new Exception("User Not Found");
            }
            await customerRepository.UpdateAsync(userId,request);
        }

        public async Task<CustomerDto> ViewProfile(ClaimsIdentity identity)
        {
            var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if(userId == null)
            {
                throw new Exception("User Not Found");
            }
            Customer customer = await customerRepository.GetById(userId);
            CustomerDto customerDto = mapper.Map<CustomerDto>(customer);
            customerDto.Email = identity.FindFirst(ClaimTypes.Email).Value;
            return customerDto;

        }
    }
}
