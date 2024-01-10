using System.ComponentModel.DataAnnotations;

namespace Identity.Models.Entities.Dto
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string EmailConfirmed { get; set; }
    }
}
