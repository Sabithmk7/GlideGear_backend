using GlideGear_backend.Models.Dtos.CategoryDtos;
using GlideGear_backend.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlideGear_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _services;
        public CategoryController(ICategoryServices services)
        {
            _services = services;
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categoryList =await _services.getCategories();
                return Ok(categoryList);
            }catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                var res = await _services.AddCategory(categoryDto);
                if (res)
                {
                    return Ok("Category successfully added");
                }
                return BadRequest("The category already exist");

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var res = await _services.RemoveCategory(id);
                if (res)
                {
                    return Ok("Category deleted succedully");
                }
                return BadRequest("Category Not found");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
