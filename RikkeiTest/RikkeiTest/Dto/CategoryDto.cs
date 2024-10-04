namespace RikkeiTest.Dto
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class CategoryView : CategoryDto 
    {
        public int Id { get; set; }
    }
}
