using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API_Empleados.Models.DTOs
{
    public class EmpleadoCrearDto
    {

        [Required(ErrorMessage = "El campo Nombre debe ser llenado")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Nombre debe ser llenado")]
        public string Apellido { get; set; }

        public string RutaImagen { get; set; }

        public IFormFile Foto  { get; set; }

        [Required(ErrorMessage = "El campo CargoId debe ser llenado")]
        public int CargoId { get; set; }

    }
}
