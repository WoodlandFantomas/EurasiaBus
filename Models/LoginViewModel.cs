using System.ComponentModel.DataAnnotations;

namespace EurasiaBus.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите email.")]
        [EmailAddress(ErrorMessage = "Введите корректный email.")]
        public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }


}
