using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Helper
{
    public class MyPasswordValidator : IPasswordValidator<Users>
    {
        List<string> CommonPassword = new List<string>() 
        { "123456" , "soheil" };

        public Task<IdentityResult> ValidateAsync(UserManager<Users> manager, Users user, string? password)
        {
            if (CommonPassword.Contains(password))
            {
                var result = IdentityResult.Failed(new IdentityError
                {
                    Code = "CommonPassword",
                    Description = "Your password Is Bad"
                });
                return Task.FromResult(result);
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
