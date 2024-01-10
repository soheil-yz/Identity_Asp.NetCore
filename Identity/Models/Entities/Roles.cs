using Microsoft.AspNetCore.Identity;

namespace Identity.Models.Entities
{
    public class Roles : IdentityRole
    {
        public string Description { get; set; }
    }
}
