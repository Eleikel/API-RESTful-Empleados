using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class UsuarioRegistrarDto
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "La contraseña debe estar entre 4 a 30 caracteres")]
        public string Password { get; set; }

    }
}
