using LoginPage.Data;
using LoginPage.DTO;
using LoginPage.Interface;
using LoginPage.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace LoginPage.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> GenerateToken(User user)
        {

            var Claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),

            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("A7xP9mQ2vR5tY8uW1kL4nB6cD3eF7gH9jK2pS5zX8nV1mC4r")
             );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: "LoginPage",
                audience: "LoginPageUsers",
                claims: Claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email) ;

            if (user == null)
            {
                return null;
            }

            if (user.LockoutEnd != null && DateTime.UtcNow < user.LockoutEnd)
            {
                return "LOCKED";
                
            }

            if (user.Password != request.Password)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 3)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(1);

                }
                await _context.SaveChangesAsync();
                return null;
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();
            return await GenerateToken(user);
        }


        public async Task<string?> RegisterAsync(RegisterDto request)
        {
            var exUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (exUser != null)
            {
                return null;
            }
            var user = new User
            {
                Email = request.Email,
                Password = request.Password,

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return "User Registered Successfully";

        }
    }
}