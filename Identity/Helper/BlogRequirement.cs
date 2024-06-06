using Identity.Models.Entities.Dto.Blog;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Helper
{
    public class BlogRequirement : IAuthorizationRequirement
    {

    }
    public class IsBlogForUserAuthorizationHandler : AuthorizationHandler<BlogRequirement, CreateBlog>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlogRequirement requirement, CreateBlog resource)
        {
            if(context.User.Identity?.Name == resource.UserName)
            {
                context.Succeed(requirement);
            }
                return Task.CompletedTask;
        }
    }
}
