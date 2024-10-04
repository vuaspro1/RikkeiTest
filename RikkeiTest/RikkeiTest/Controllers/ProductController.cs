using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly MyDbContext myDbContext;
        private readonly ICategoryRepository categoryRepository;
        public ProductController(IProductRepository productRepository, IMapper mapper, MyDbContext myDbContext, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.myDbContext = myDbContext;
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult GetProducts(int page = 0, int pageSize = 10) 
        {
            try
            {
                var result = productRepository.GetProducts(page, pageSize != 0 ? pageSize : 10);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Can't get the product");
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult CreateProduct([FromBody] ProductDto productCreate)
        {
            if (productCreate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productMap = mapper.Map<Product>(productCreate);
            productMap.Category = categoryRepository.GetCategory(productCreate.CategoryId);

            if (!productRepository.CreateProduct(productMap))
            {
                return BadRequest("Error");
            }
            return Ok(productMap);
        }
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto productUpdate)
        {
            if (productUpdate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var existingProduct = myDbContext.Products.FirstOrDefault(item => item.Id == id);
            if (existingProduct == null)
                return NotFound();

            mapper.Map(productUpdate, existingProduct);
            existingProduct.Category = categoryRepository.GetCategory(productUpdate.CategoryId);

            if (!productRepository.UpdateProduct(existingProduct))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }
            return Ok(productUpdate);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteFilm(int id)
        {
            if (!productRepository.ProductExists(id))
            {
                return NotFound();
            }

            productRepository.DeleteProduct(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("Successfully");
        }
    }
}
