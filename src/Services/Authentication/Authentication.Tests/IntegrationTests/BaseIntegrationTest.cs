using System.Net.Http.Headers;
using System.Net.Http.Json;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Contexts;
using Authentication.DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Tests.IntegrationTests
{
    public abstract class BaseIntegrationTest
        : IClassFixture<CustomWebApplicationFactory>,
            IDisposable
    {
        protected readonly AuthContext _dbContext;
        protected readonly HttpClient Client;
        protected readonly UserManager<User> _userManager;

        protected static string AdminJwtToken = null!;
        protected static string ModeratorJwtToken = null!;
        protected static string UserJwtToken = null!;
        private static bool tokensInitialized = false;

        protected BaseIntegrationTest(CustomWebApplicationFactory factory)
        {
            _dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AuthContext>();
            _userManager = factory.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
            Client = factory.CreateClient();

            if (!tokensInitialized)
            {
                AdminJwtToken = AuthenticateAdminAsync().GetAwaiter().GetResult();
                ModeratorJwtToken = AuthenticateModeratorAsync().GetAwaiter().GetResult();
                UserJwtToken = AuthenticateUserAsync().GetAwaiter().GetResult();
                tokensInitialized = true;
            }
        }

        private async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var loginRequest = new UserRequestDto
            {
                Email = email,
                Password = password
            };

            var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);

            response.EnsureSuccessStatusCode();
            
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            return loginResponse.JwtToken;
        }

        private async Task<string> AuthenticateAdminAsync()
        {
            return await AuthenticateUserAsync("admin@gmail.com", "adminPassword");
        }

        private async Task<string> AuthenticateModeratorAsync()
        {
            return await AuthenticateUserAsync("moderator@gmail.com", "moderatorPassword");
        }

        private async Task<string> AuthenticateUserAsync()
        {
            return await AuthenticateUserAsync("user@gmail.com", "userPassword");
        }

        protected void AddJwtTokenToHeader(string jwtToken)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }
        
        protected async Task<User> AddUserToDbAsync(UserRequestDto requestDto)
        {
            var user = new User()
            {
                Email = requestDto.Email,
                UserName = requestDto.Email.ToUpper()
            };
            await _userManager.CreateAsync(user, requestDto.Password);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        
        protected async Task AssignUserRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Client.Dispose();
            _dbContext.Dispose();
        }
    }
}