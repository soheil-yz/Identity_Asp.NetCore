using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Entities.Dto.Accounts
{
    public class TwoFactorLoginDto
    {
        [Required]
        public string Code { get; set; }
        public bool IsPersistent { get; set; }
        public string Provider { get; set; }
    }
}
