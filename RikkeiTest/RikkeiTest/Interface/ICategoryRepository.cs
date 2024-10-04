using RikkeiTest.Dto;
using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface ICategoryRepository
    {
        PaginationDTO<CategoryView>GetCategorys(int  page, int pageSize);
        Category GetCategory(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int id);
        bool CategoryExists(int id);
    }
}
