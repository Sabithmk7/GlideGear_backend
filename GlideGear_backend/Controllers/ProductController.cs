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

        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _services.GetProducts();
                return Ok(products);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var p = await _services.GetProductById(id);
                return Ok(p);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getByCategory")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            try
            {
                var p = await _services.GetProductByCategory(categoryName);
                return Ok(p);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool res = await _services.DeleteProduct(id);
                if (res)
                {
                    return Ok("Product deleted succesfully");
                }
                return BadRequest("No product found");
                
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost("Add")]
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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm]ProductDto productDto,IFormFile image)
        {
            try
            {
                await _services.UpdateProduct(id, productDto, image);
                return Ok("Updated seccefully");
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("search-item")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            try
            {
                var res = await _services.SearchProduct(search);
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("paginated-products")]
        public async Task<IActionResult> pagination([FromQuery]int pageNumber=1, [FromQuery] int size=10)
        {
            try
            {
                var res = await _services.ProductPagination(pageNumber, size);
                return Ok(res);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
