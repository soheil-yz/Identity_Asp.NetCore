using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = Microsoft.Build.Framework.RequiredAttribute;

namespace Identity.Models.Entities.Dto.Accounts
{
    public class SetPhoneNumberDto
    {
        [Required]
        [RegularExpression(@"(\+98|0)?9\d{9}")]
        public string PhoneNumber { get; set; }
    }
}
