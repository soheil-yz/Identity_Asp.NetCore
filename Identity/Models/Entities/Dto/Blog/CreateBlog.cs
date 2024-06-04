using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Models.Entities.Dto.Blog
{
    public class CreateBlog
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        [BindNever]
        public string UserId { get; set; }
    }
}
