using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlideGear_backend.Services.JwtService
{
    public class JwtServices:IJwtServices
    {
        private readonly string key;
        public JwtServices(IConfiguration configuration)
        {
            key = configuration["Jwt:Key"];
        }

        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = Encoding.UTF8.GetBytes(key);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    throw new SecurityTokenException("Invalid Jwt Tokwn.");
                }

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);


                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var Id))
                {
                    throw new SecurityTokenException("Invalid or missing user Id claim.");
                }
                return Id;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while fetching id from token {ex.Message}");
                return 0;
            }
        }

    }
}
