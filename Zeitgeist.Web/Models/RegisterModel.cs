using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Models
{
    public class RegisterModel
    {

        public RegisterModel()
        {
            
        }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Pregunta Secreta")]
        public string PasswordQuestion { get; set; }

        [Required]
        [Display(Name = "Respuesta Pregunta Secreta")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0}.")]
        public string PasswordAnswers { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public int Peso { get; set; }
        public decimal Estatura { get; set; }
        public List<SelectListItem> AvailableQuestions { get; set; }
        public List<SelectListItem> AvailableSex { get; set; }
    }
}