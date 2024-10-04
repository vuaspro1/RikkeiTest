namespace RikkeiTest.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

    }
    public class ProductView : ProductDto
    {
        public int Id { get; set; }
    }
}
