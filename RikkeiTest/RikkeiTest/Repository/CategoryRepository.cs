using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RikkeiTest.Dto;
using RikkeiTest.Interface;
using RikkeiTest.Models;
using System.Data;

namespace RikkeiTest.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext myDbContext;
        private readonly IMapper mapper;
        public CategoryRepository(MyDbContext myDbContext, IMapper mapper)
        {
            this.myDbContext = myDbContext;
            this.mapper = mapper;
        }
        public bool CreateCategory(Category category)
        {
            myDbContext.Add(category);
            return Save();
        }
        public bool Save()
        {
            var saved = myDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
        public PaginationDTO<CategoryView> GetCategorys(int page, int pageSize)
        {
            PaginationDTO<CategoryView> pagination = new PaginationDTO<CategoryView>();
            var categorys = myDbContext.Categorys.OrderBy(c => c.Id);
            var category = categorys.Select(item => new CategoryView
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
            }).ToList();
            var result = PaginatedList<CategoryView>.Create(category.AsQueryable(), page, pageSize);

            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = categorys.Count();
            pagination.pageSize = pageSize;
            return pagination;

        }

        public bool UpdateCategory(Category category)
        {
            myDbContext.Update(category);
            return Save();
        }

        public bool DeleteCategory(int id)
        {
            var deleteCategory = myDbContext.Categorys.SingleOrDefault(item => item.Id == id);
            var deleteProduct = myDbContext.Products.Where(item => item.Category.Id == id).ToList();

            if (deleteCategory != null)
            {
                if (deleteProduct.Any())
                {
                    myDbContext.Products.RemoveRange(deleteProduct);
                }
                myDbContext.Categorys.Remove(deleteCategory);
            }
            return Save();
        }

        public bool CategoryExists(int id)
        {
            return myDbContext.Categorys.Any(c => c.Id == id);
        }

        public Category GetCategory(int id)
        {
            var category = myDbContext.Categorys.Where(item => item.Id == id).FirstOrDefault();
            return mapper.Map<Category>(category);
        }
    }
}
