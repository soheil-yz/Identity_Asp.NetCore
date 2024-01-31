using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.Areas.Admin.Models.Dto.Roles
{
    public class AddUserRoleDto
    {
        public string Id { get; set; }
        public string Role {  get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
