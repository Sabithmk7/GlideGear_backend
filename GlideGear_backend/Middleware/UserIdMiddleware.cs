using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GlideGear_backend.Middleware
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserIdMiddleware> _logger;

        public UserIdMiddleware(RequestDelegate next, ILogger<UserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            if (context.User.Identity?.IsAuthenticated == true)
            {
                
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim != null)
                {
                    context.Items["UserId"] = idClaim.Value; 
                }
                else
                {
                    _logger.LogWarning("No 'NameIdentifier' claim found in the JWT token.");
                }
            }
            else
            {
                //_logger.LogWarning("User is not authenticated.");
            }

            await _next(context);
        }
    }
}
