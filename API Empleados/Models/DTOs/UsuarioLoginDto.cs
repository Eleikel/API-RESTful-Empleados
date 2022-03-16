using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatorio")]
        public string Password { get; set; }
    }
}
