using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using RikkeiTest.Repository;
using System.Security.Claims;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly MyDbContext myDbContext;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, MyDbContext myDbContext)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public IActionResult GetCategorys(int page = 0, int pageSize = 10)
        {
            try
            {
                var result = categoryRepository.GetCategorys(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Can't get the Category.");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest();
            var category = myDbContext.Categorys
                .Where(item => item.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "This category already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = mapper.Map<Category>(categoryCreate);

            if (!categoryRepository.CreateCategory(categoryMap))
            {
                return BadRequest("Error");
            }
            return Ok(categoryMap);
        }
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateCategory(int id,[FromBody] CategoryDto categoryUpdate) 
        {
            if (categoryUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var existingCategory = myDbContext.Categorys.FirstOrDefault(item => item.Id == id);
            if (existingCategory == null)
                return NotFound();

            mapper.Map(categoryUpdate, existingCategory);

            if (!categoryRepository.UpdateCategory(existingCategory))
            {
                return BadRequest("Error");
            }
            return Ok("Successfully");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteCategory([FromRoute] int id)
        {
            if (!categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            categoryRepository.DeleteCategory(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
