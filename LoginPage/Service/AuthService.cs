using LoginPage.Data;
using LoginPage.DTO;
using LoginPage.Interface;
using LoginPage.Model;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IConfiguration _config;
        private readonly IEmailService _eService;

        public AuthService(AppDbContext context, IConfiguration config, IEmailService eSerivce)
        {
            _context = context;
            _config = config;
            _eService = eSerivce;
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("purpose", "reset-password"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]!)
                );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: _config["JWT:PasswordResetIssuer"],
                audience: _config["JWT:PasswordResetAudince"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
                    user.LockoutEnd = DateTime.UtcNow.AddHours(1);

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

        public async Task<string?> ForgotPasswordAsync(ForgotPasswordDto forgot)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == forgot.Email);
          
            if (user == null)
            {
                return null;
            }

            var resetToken = await GeneratePasswordResetToken(user);

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(10);
            await _context.SaveChangesAsync();




            var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";
                
            var emailDto = new EmailDto
            {
                To = user.Email,
                Subject = "Reset Password",
                Body = resetLink
            };

            _eService.SendEmailAsync(emailDto);

            return "Success";
        }

        public async Task<string?> ResetPassword(ResetPasswordDto reset)
        {
            if (string.IsNullOrEmpty(reset.ResetToken))
            {
                return "Reset Token is requrired";
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();


                tokenHandler.ValidateToken(reset.ResetToken,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = _config["JWT:PasswordResetIssuer"],
                        ValidAudience = _config["JWT:PasswordResetAudince"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_config["JWT:Key"]!)
                            )
                    },
                    out SecurityToken validatedToken

                    );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return "Email not found in token";
                }

                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

                if(user == null)
                {
                    return "User not Found";
                }

                if (string.IsNullOrEmpty(reset.ConfirmPassword))
                {
                    return "Confirm password is required";
                }

                if (string.IsNullOrEmpty(reset.NewPassword))
                {
                    return "New password is required";
                }

                if (reset.NewPassword != reset.ConfirmPassword)
                {
                    return "Password do not matched";
                }


                user.Password = reset.NewPassword;
                await _context.SaveChangesAsync();
                return "Password Reset Succesfuly";
            }
            catch
            {
                return "Invalid or Expired Token";
            }


           


        }


    }
}