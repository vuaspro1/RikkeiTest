namespace RikkeiTest.Dto
{
    public class PermissionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class PermissionView : PermissionDto 
    {
        public int Id { get; set; }
    }
}
