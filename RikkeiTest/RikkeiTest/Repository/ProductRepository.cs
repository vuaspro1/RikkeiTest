using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;

namespace RikkeiTest.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext myDbContext;
        private readonly IMapper mapper;
        public ProductRepository(MyDbContext myDbContext, IMapper mapper)
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
        }

        public bool CreateProduct(Product product)
        {
            myDbContext.Add(product);
            return Save();
        }

        public bool DeleteProduct(int id)
        {
            var deleteProduct = myDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (deleteProduct != null) 
            {
                myDbContext.Products.Remove(deleteProduct);
            }
            return Save();
        }

        public PaginationDTO<ProductView> GetProducts(int page, int pageSize)
        {
            PaginationDTO<ProductView> pagination = new PaginationDTO<ProductView>();
            var products = myDbContext.Products.Include(item => item.Category).ToList();
            var product = products.Select(item => new ProductView
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CategoryId = item.Category != null ? item.Category.Id : 0,
                CategoryName = item.Category != null ? item.Category.Name : "Unknown",
            }).ToList();
            var result = PaginatedList<ProductView>.Create(product.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = product.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public bool UpdateProduct(Product product)
        {
            myDbContext.Update(product);
            return Save();
        }

        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ProductExists(int id)
        {
            return myDbContext.Products.Any(f => f.Id == id);
        }
    }
}
