using APIPractice.Exceptions;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace APIPractice.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IRegisterUserRepository userRepository;

        public AuthService(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, IRegisterUserRepository userRepository) 
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
        }

        public async Task RegisterCustomer(RegisterCustomerRequest registerCustomerRequest)
        {
            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerCustomerRequest.Email,
                UserName = registerCustomerRequest.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerCustomerRequest.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(identityUser, "customer");
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(identityUser);
                throw new Exception("Failed to assign role to user.");
            }

            try
            {
                await userRepository.AddCustomer(registerCustomerRequest, identityUser);
            }
            catch (Exception)
            {
                await userManager.DeleteAsync(identityUser);
                throw new NotFoundException("The phone number already exists.");
            }
        }

        public async Task RegisterStaff(RegisterStaffRequest registerStaffRequest)
        {
            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerStaffRequest.Email,
                UserName = registerStaffRequest.Email
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerStaffRequest.Password);
            if (identityResult.Succeeded)
            {
                if (registerStaffRequest.Role != null && (registerStaffRequest.Role.ToLower().Equals("manager") || registerStaffRequest.Role.ToLower().Equals("employee")))
                {
                    identityResult = await userManager.AddToRoleAsync(identityUser, registerStaffRequest.Role);
                    if (registerStaffRequest.Role.ToLower() == "manager")
                    {
                        await userRepository.AddManager(registerStaffRequest, identityUser);
                    }
                    else if (registerStaffRequest.Role.ToLower() == "employee")
                    {
                        await userRepository.AddEmployee(registerStaffRequest, identityUser);
                    }
                }
                else
                {
                    throw new Exception("Role not specified");
                }
            }
            else
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
        }

        public async Task<LoginResponseDto> LoginUser(LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (checkPassword)
                {
                    var role = await userManager.GetRolesAsync(user);
                    if (role != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, role.First());
                        var response = new LoginResponseDto { JwtToken = jwtToken };
                        return response;
                    }
                }
                else
                {
                    throw new Exception("Wrong Password");
                }
            }
            throw new Exception("Couldn't find Email Address");
        }
    }
}
