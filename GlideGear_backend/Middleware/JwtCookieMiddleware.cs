namespace GlideGear_backend.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Cookies.TryGetValue("AuthToken",out var token))
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Append("Authorization", $"Bearer {token}");
                    Console.WriteLine($"{token}");
                }
            }
            await _next(context);
        }
        
    }
}
