using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface IAuthService
    {
        Task RegisterCustomer(RegisterCustomerRequest registerCustomer);
        Task RegisterStaff(RegisterStaffRequest registerStaff);
        Task<LoginResponseDto> LoginUser(LoginRequest loginUserRequest);
    }
}
