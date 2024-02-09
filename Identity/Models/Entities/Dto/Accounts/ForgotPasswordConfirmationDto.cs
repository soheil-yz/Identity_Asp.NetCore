using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Entities.Dto.Accounts
{
    public class ForgotPasswordConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
