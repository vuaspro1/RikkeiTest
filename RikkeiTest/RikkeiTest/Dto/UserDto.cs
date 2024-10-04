namespace RikkeiTest.Dto
{
    public class UserDto : UserUpdate
    {
        public string UserName { get; set; }
    }

    public class UserUpdate 
    {
        public string Name { get; set; }
        public string Password { get; set; }

    }
    public class UserView : UserDto 
    {
        public int Id { get; set; }
        public List<RoleView> Roles { get; set; }
    }
}
