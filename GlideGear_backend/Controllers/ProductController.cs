using GlideGear_backend.Models.Dtos.ProductDtos;
using GlideGear_backend.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _services;
        public ProductController(IProductServices services)
        {
            _services = services;
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto productDto, IFormFile image)
        {
            try
            {
                await _services.AddProduct(productDto, image);
                return Ok("Product Succesfully added");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
