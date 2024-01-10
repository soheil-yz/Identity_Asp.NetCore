using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class DataBaseContext : IdentityDbContext<Users,Roles,string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { 
        }

    }
}
