using System.ComponentModel.DataAnnotations;

namespace Zeitgeist.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuario o contraseña incorrecta",AllowEmptyStrings = false)]
        [Display(Name = "Nombre de usuario")]

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}