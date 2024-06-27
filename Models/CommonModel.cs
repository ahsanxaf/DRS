namespace DRS.Models
{
    public class CommonModel
    {
        public Role Roles { get; set; }
        public Permission permissions { get; set; }

        public List<User> userslist { get; set; }

        public User user { get; set; }

        public List<Role> roleslist { get; set; }
        public List<Role> GetRoles { get; set; }

        public Permission GetPermissions { get; set; }   

        public LawCategory lawCategory { get; set; }

        public List<LawCategory> lawCategories { get; set; } 
    }
}
