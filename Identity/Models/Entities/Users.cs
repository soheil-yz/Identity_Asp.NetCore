using Microsoft.AspNetCore.Identity;

namespace Identity.Models.Entities
{
    public class Users : IdentityUser
    {
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        public ICollection<Blog> Blogs { get; set; }

         
    }
    
}
