namespace RikkeiTest.Dto
{
    public class RoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class RoleView : RoleDto 
    {
        public int Id { get; set; }
    }
}
