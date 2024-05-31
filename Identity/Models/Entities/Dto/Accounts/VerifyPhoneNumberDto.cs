using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Entities.Dto.Accounts
{
    public class VerifyPhoneNumberDto
    {
        public string PhoneNumber { get; set; }
        [Required]
        public string code { get; set; }
    }
}
