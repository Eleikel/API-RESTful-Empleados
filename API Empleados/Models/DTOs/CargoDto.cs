using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Empleados.Models.DTOs
{
    public class CargoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debes introducir el Nombre del Cargo")]
        public string NombreCargo { get; set; }
        [Required(ErrorMessage = "Debes introducir el ID del Departamento el cual pertenece este cargo")]
        public int departamentoId { get; set; }
        public Departamento Departamento { get; set; }
    }
}
