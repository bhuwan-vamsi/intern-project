using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using AutoMapper;
using System.Security.Claims;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IMapper mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        this.customerRepository = customerRepository;
        this.mapper = mapper;
    }

    public async Task<CustomerDto> ViewProfile(ClaimsIdentity identity)
    {
        var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) throw new Exception("User Not Found");

        var userId = Guid.Parse(userIdClaim);
        var customer = await customerRepository.GetById(userId);

        var customerDto = mapper.Map<CustomerDto>(customer);
        customerDto.Email = identity.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

        return customerDto;
    }

    public async Task EditProfile(ClaimsIdentity identity, EditProfileRequest request)
    {
        var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) throw new Exception("User Not Found");

        var userId = Guid.Parse(userIdClaim);
        await customerRepository.UpdateAsync(userId, request);
    }
}
