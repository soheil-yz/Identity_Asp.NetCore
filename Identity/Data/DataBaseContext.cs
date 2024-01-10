using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    public class DataBaseContext : IdentityDbContext<Users, Roles, string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<string>>().HasKey(p => new
            { p.ProviderKey, p.LoginProvider });
            builder.Entity<IdentityUserRole<string>>().HasKey(p => new
            { p.UserId, p.RoleId});            
            builder.Entity<IdentityUserToken<string>>().HasKey(p => new
            { p.UserId, p.LoginProvider , p.Name});

            builder.Entity<Users>().Ignore(p=> p.NormalizedEmail);
            //base.OnModelCreating(builder);  
        }
    }
}
