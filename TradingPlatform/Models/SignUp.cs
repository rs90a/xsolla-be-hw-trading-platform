using System.ComponentModel.DataAnnotations;
using TradingPlatform.Enum;

namespace TradingPlatform.Models
{
    public class SignUp
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Введенные пароли не совпадают")]
        public string PasswordConfirm { get; set; }
        
        [Required]
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }
    }
}