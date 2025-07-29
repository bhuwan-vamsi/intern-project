using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
                UserName = registerCustomerRequest.UserName,
                Email = registerCustomerRequest.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerCustomerRequest.Password);
            if (identityResult.Succeeded)
            {
                if(registerCustomerRequest.Role != null && registerCustomerRequest.Role.Any())
                {
                    identityResult = await userManager.AddToRoleAsync(identityUser, registerCustomerRequest.Role);
                    if (identityResult.Succeeded)
                    {
                        await userRepository.AddCustomer(registerCustomerRequest, identityUser);
                    }
                    else
                    {
                        throw new Exception("Invalid Role");
                    }
                }
                else
                {
                    throw new Exception("Role not specified");
                }
            }
            else
            {
                throw new Exception("Username already exists");
            }


        }

        public async Task<LoginResponseDto> LoginUser(LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.UserName);
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
            }
            throw new Exception("Something Went Wrong");
        }

        
    }
}
