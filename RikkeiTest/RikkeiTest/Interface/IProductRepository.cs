using RikkeiTest.Dto;
using RikkeiTest.Models;

namespace RikkeiTest.Interface
{
    public interface IProductRepository
    {
        PaginationDTO<ProductView> GetProducts(int page, int pageSize);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int id);
        bool ProductExists(int  id);
    }
}
