using AuthenticationService.DbSets;
using AuthenticationService.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    public class AuthenticationService : AuthenticateService.AuthenticateServiceBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, IUserRepository userRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null && !await _userManager.CheckPasswordAsync(user, request.Password))
                return new LoginResponse { Token = null };

            var userClaims = await _userManager.GetClaimsAsync(user);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAStringForJwtWhichIsEnoughLong"));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT: ValidIssuer"],
                audience: _configuration["JWT: ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: userClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var userExists = await _userManager.FindByNameAsync(request.Username);
            if (userExists is not null)
                return new RegisterResponse
                {
                    ResponseStatus = ResponseStatus.UserExists
                };

            User user = new User()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username,
                Name = request.Name,
                Surname = request.Surname
            };

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim("username", request.Username),
                new Claim("userId", user.Id)
            };

            await _userRepository.Add(user);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new RegisterResponse
                {
                    ResponseStatus = ResponseStatus.Error
                };

            await _userManager.AddClaimsAsync(user, claims);

            await _userRepository.Save();

            return new RegisterResponse
            {
                ResponseStatus = ResponseStatus.Ok
            };
        }
    }
}
