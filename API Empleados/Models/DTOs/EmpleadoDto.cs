using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class EmpleadoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre debe ser llenado")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Nombre debe ser llenado")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo Ruta de Imagen debe ser llenado")]
        public string RutaImagen { get; set; }

        //Foreing
        public int CargoId { get; set; }

        public Cargo Cargo { get; set; }
    }
}
