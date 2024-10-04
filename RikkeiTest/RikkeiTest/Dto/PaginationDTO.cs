namespace RikkeiTest.Dto
{
    public class PaginationDTO<T>
    {
        public List<T> data { get; set; }
        public int totalItem { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
