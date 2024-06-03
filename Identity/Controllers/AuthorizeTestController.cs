using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Authorize(Roles ="Admin , Operator")]
    public class AuthorizeTestController : Controller
    {
        public string Index()
        {
            return "Hi soheil Yazdanii ";
        }
        [AllowAnonymous] 
        public string Edit()
        {
            return "Hi soheil Yazdanii ";
        }
    }
}
