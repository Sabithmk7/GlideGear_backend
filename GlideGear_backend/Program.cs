
using GlideGear_backend.DbContexts;
using GlideGear_backend.Mapper;
using GlideGear_backend.Middleware;
using GlideGear_backend.Services.CartServices;
using GlideGear_backend.Services.CategoryServices;
using GlideGear_backend.Services.CloudinaryServices;
using GlideGear_backend.Services.JwtService;
using GlideGear_backend.Services.OrderSerices;
using GlideGear_backend.Services.ProductServices;
using GlideGear_backend.Services.Users;
using GlideGear_backend.Services.UserServices;
using GlideGear_backend.Services.WhishListServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GlideGear_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            //_______Service class registration_____

            builder.Services.AddScoped<IAuthServices, AuthSevices>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<IProductServices, ProductService>();
            //builder.Services.AddScoped<IJwtServices, JwtServices>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IWhishListService, WhishListService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            //Jwt authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("ReactPolicy");
            app.UseMiddleware<JwtCookieMiddleware>();
            app.UseStaticFiles();
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
